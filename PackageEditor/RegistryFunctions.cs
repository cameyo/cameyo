using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using PackageEditor.RegFileReader;

namespace PackageEditor
{
  class RegistryFunctions
  {
    StringBuilder errors = new StringBuilder();
    Dictionary<String, String> virtualKeys = new Dictionary<string, string>();
    public RegistryFunctions()
    {
      virtualKeys.Add("HKEY_CURRENT_USER\\Software\\Classes", "%CurrentUser%_Classes");
      virtualKeys.Add("HKEY_CURRENT_USER", "%CurrentUser%");
      virtualKeys.Add("HKEY_LOCAL_MACHINE", "MACHINE");
      virtualKeys.Add("HKEY_USERS", "USER");
    }

    class TRegistrySubstitute
    {
      public String originalKey;
      public String virtualKey;
      public TRegistrySubstitute(String originalKey, String virtualKey)
      {
        this.originalKey = originalKey;
        this.virtualKey = virtualKey;
      }
    }

    public void ExportVirtualRegistryToFile(string RegFileName, RegistryKey key, String virtKey)
    {
      int rootKeyLen = key.Name.Length + 1;
      if (!String.IsNullOrEmpty(virtKey))
        key = key.OpenSubKey(virtKey);

      FileStream fs = new FileStream(RegFileName, FileMode.Create);
      BufferedStream bfs = new BufferedStream(fs);
      StreamWriter sw = new StreamWriter(bfs);

      sw.WriteLine("Windows Registry Editor Version 5.00");
      sw.WriteLine("");
      writeRegKeys(sw, rootKeyLen, key);

      sw.Close();
      bfs.Close();
      fs.Close();

      if (errors.Length > 0)
      {
        MessageBox.Show("The following errors occured while exporting the registry: "+errors);
      }
    }

    void writeRegKeys(StreamWriter writer, int rootKeyLen, RegistryKey key)
    {
      writeRegKeyValues(writer, rootKeyLen, key);
      string[] subkeys = key.GetSubKeyNames();
      for (int t = 0; t < subkeys.Length; t++)
      {
        RegistryKey subkey = key.OpenSubKey(subkeys[t]);
        String keyName = subkey.Name.Substring(rootKeyLen);
        writeRegKeys(writer, rootKeyLen, subkey);
        subkey.Close();
      }
    }

    void writeRegKeyValues(StreamWriter sw, int rootKeyLen, RegistryKey key)
    {
      //string keyname = key.Name;      
      string keyname = "";
      if (rootKeyLen > key.Name.Length)
        return;//should only happen for the vos root  HKCU\..TempVirtReg\myApp\registry
      keyname = key.Name.Substring(rootKeyLen);
      if (String.IsNullOrEmpty(keyname))
        return;

      bool found = false;
      string keynameCheck = keyname + '\\';
      foreach (KeyValuePair<String,String> rs in virtualKeys)
      {
        if (keynameCheck.StartsWith(rs.Value, StringComparison.InvariantCultureIgnoreCase))
        {
          keyname = keyname.Substring(rs.Value.Length).Insert(0, rs.Key);
          found = true;
          break;
        }
      }
      if (!found)
      {
        MessageBox.Show(String.Format("Not yet implemented registry key: {0}", keyname));
        return;
      }
      sw.WriteLine("[{0}]", keyname);
      foreach (string valuename in key.GetValueNames())
      {
        object value = null;
        String regvalue = null;
        RegistryValueKind kind = key.GetValueKind(valuename);

        string regvaluename = valuename.Replace("\\", "\\\\");
        regvaluename = regvaluename.Replace("\"", "\\\"");
        if (regvaluename.Length == 0)
          regvaluename = "@";
        else
          regvaluename = "\"" + regvaluename + "\"";

        if (kind != RegistryValueKind.Unknown)
        {
          value = key.GetValue(valuename);
        }

        switch (kind)
        {
          case RegistryValueKind.Binary:
            System.Byte[] x = (System.Byte[])value;
            regvalue = RegFileHelper.ByteArrayToHexComma(x);
            RegFile.ByteArrayFormatString(ref regvalue, regvaluename.Length + 5);
            regvalue = "hex:" + regvalue;
            break;
          case RegistryValueKind.DWord:
            regvalue = string.Format("dword:{0:x8}", value);
            break;
          case RegistryValueKind.ExpandString:
            byte[] bytes = Encoding.Unicode.GetBytes((String)value);
            regvalue = RegFileHelper.ByteArrayToHexComma(bytes);
            regvalue += (regvalue != "" ? "," : "") + "00,00";
            RegFile.ByteArrayFormatString(ref regvalue, regvaluename.Length + 8);
            regvalue = "hex(2):" + regvalue;
            break;
          case RegistryValueKind.MultiString:
            string[] sa = (string[])value;
            regvalue = "";
            for (int m = 0; m < sa.Length; m++)
            {
              bytes = Encoding.Unicode.GetBytes((String)sa[m]);
              regvalue += RegFileHelper.ByteArrayToHexComma(bytes);
              regvalue = regvalue + ",00,00,";
            }
            regvalue = regvalue + "00,00";
            RegFile.ByteArrayFormatString(ref regvalue, regvaluename.Length + 8);
            regvalue = "hex(7):" + regvalue;
            break;
          case RegistryValueKind.QWord:
            {
              long l = (long)value;
              bytes = System.BitConverter.GetBytes(l);
              regvalue += RegFileHelper.ByteArrayToHexComma(bytes);
              regvalue = "hex(b):" + regvalue;
              break;
            }
          case RegistryValueKind.String:
            regvalue = ((string)value).Replace("\\", "\\\\");
            regvalue = regvalue.Replace("\"", "\\\"");
            regvalue = string.Format("\"{0}\"", regvalue);
            break;
          case RegistryValueKind.Unknown:
            regvalue = "hex(0):";
            break;
          default:
            errors.Append(String.Format("\r\nWARNING: RegistryFunctions.enumRegKeyValues  RegistryValueKind.{0} not supported", kind));
            break;
        }
        sw.WriteLine("{0}={1}", regvaluename, regvalue);
      }
      sw.WriteLine("");
    }

    public void ImportVirtualRegistryFromFile(string RegFileName, RegistryKey key)
    {
      RegistryKey writeKey = null;
      RegFile regFile = new RegFile(RegFileName);

      RegFileRegistryKey regFileRegistryKey;
      KeyValuePair<string, string> virtReplaceKey = new KeyValuePair<string,string>("-","");
      while (regFile.readKey(out regFileRegistryKey))
      {
        if (writeKey != null)
          writeKey.Close();
        writeKey = null;

        if (!regFileRegistryKey.keyName.StartsWith(virtReplaceKey.Key+'\\'))
        {
          virtReplaceKey = new KeyValuePair<string, string>("-", "");
          foreach (KeyValuePair<string, string> value in virtualKeys)
          {
            if (regFileRegistryKey.keyName.StartsWith(value.Key))
            {
              virtReplaceKey = value;
              break;
            }
          }
        }
        if (virtReplaceKey.Key != "-")
        {
          String virtKey = regFileRegistryKey.keyName.Remove(0, virtReplaceKey.Key.Length).Insert(0, virtReplaceKey.Value);
          writeKey = key.CreateSubKey(virtKey);
        }
        else
          regFile.errors.Append("\r\n\r\n#### Registry root key not supported in current import:" + regFileRegistryKey.keyName + " ####\r\n");

        if (writeKey != null)
        {
          foreach (RegFileRegistryValue regFileRegistryValue in regFileRegistryKey.values)
          {
            if (regFileRegistryValue.type != RegistryValueKind.Unknown)
            {
              writeKey.SetValue(regFileRegistryValue.name, regFileRegistryValue.value, regFileRegistryValue.type);
            }
          }
        }
      }
      regFile.Close();

      if (regFile.errors.Length > 0)
      {
        MessageBox.Show("The following errors occured while importing the .reg file: " + regFile.errors);
      }
    }
  }
}
