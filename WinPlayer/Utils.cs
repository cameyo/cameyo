using System;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace Cameyo
{
    // Utils
    public class Utils
    {
        public static void RequireTls12()
        {
            // Fixes a compatibility issue on machines with old protocol configuration (Chad @ Bentley). To reproduce:
            //   [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client]
            //   "Enabled"=dword:00000000
            //   [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server]
            //   "Enabled"=dword:00000000
            // => System.ComponentModel.Win32Exception: The client and server cannot communicate, because they do not possess a common algorithm
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
#if WINPLAYER35
#else
            ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
#endif
        }

        public static string MyPath(string fileName)
        {
            var myPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            return Cameyo.Utils.PathCombine(myPath, fileName);
        }

        static public bool KillFile(string fileName)
        {
            try
            {
                File.SetAttributes(fileName, 0);
                File.Delete(fileName);
            }
            catch { }
            return (!File.Exists(fileName));
        }

        public static bool Deltree(string target_dir)
        {
            try
            {
                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    Utils.KillFile(file);
                }

                foreach (string dir in dirs)
                {
                    Deltree(dir);
                }

                Directory.Delete(target_dir, false);
            }
            catch { }

            return (!Directory.Exists(target_dir));
        }

        static public bool ShellExec(string cmd, string args, ref int exitCode, bool wait, bool hidden)
        {
            try
            {
                var procStartInfo = new ProcessStartInfo(cmd, args);
                Process proc = new Process();
                if (hidden)
                {
                    procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    procStartInfo.CreateNoWindow = true;
                }
                procStartInfo.UseShellExecute = true;
                proc.StartInfo = procStartInfo;
                if (!proc.Start())
                    return false;
                if (wait)
                {
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool ShellExec(string cmd, string args, bool hidden)
        {
            int exitCode = 0;
            return ShellExec(cmd, args, ref exitCode, false, hidden);
        }

        static public bool ShellExec(string cmd, bool hidden)
        {
            return ShellExec(cmd, null, hidden);
        }

        static public bool SilentInstallAvailable(String silentInstallExe, String installerFile)
        {
            if (installerFile.EndsWith(".msi", StringComparison.InvariantCultureIgnoreCase))
                return true;
            int exitCode = 0;
            bool bExec = ShellExec(silentInstallExe, "/detect \"" + installerFile + "\"", ref exitCode, true, true);
            return (bExec && exitCode == 0);
        }

        // CombinePath: a safer alternative to Path.Combine().
        // To my great surprise & sadness, Path.Combine("c:\\now", "\\anything") -> "\\anything"
        // In other words, whenever path2 starts with '\' or '/', it suppresses path1, which is a security issue!
        public static string PathCombine(string path1, string path2, string path3 = null)
        {
            // 1 param
            if (path2 == null)
                return path1;

            // 2+ params
            if (!string.IsNullOrEmpty(path1) && path1.EndsWith(":"))
                path1 += "\\";
            while (!string.IsNullOrEmpty(path2) && (path2.StartsWith("\\") || path2.StartsWith("/")))
                path2 = path2.Remove(0, 1);
            if (path3 == null)
                return System.IO.Path.Combine(path1 ?? "", path2);

            // 3 params
            while (!string.IsNullOrEmpty(path3) && (path3.StartsWith("\\") || path3.StartsWith("/")))
                path3 = path3.Remove(0, 1);
            return System.IO.Path.Combine(path1 ?? "", path2, path3);
        }

        static public String BytesToStr(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.#} {1}", len, sizes[order]);
        }

        static public String HexDump(byte[] bytes)
        {
            string hexString = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                hexString += bytes[i].ToString("X2");
            }
            return hexString;
        }

        public static string UrlEncode(string value)
        {
            value = Uri.EscapeDataString(value);

            StringBuilder builder = new StringBuilder();
            foreach (char ch in value)
            {
                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~%".IndexOf(ch) != -1)
                {
                    builder.Append(ch);
                }
                else
                {
                    builder.Append('%' + string.Format("{0:X2}", (int)ch));
                }
            }
            return builder.ToString();
        }

        static public string TimeAgo(DateTime dt)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 0)
            {
                return "not yet";
            }
            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 2 * MINUTE)
            {
                return "a minute ago";
            }
            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 90 * MINUTE)
            {
                return "an hour ago";
            }
            if (delta < 24 * HOUR)
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 48 * HOUR)
            {
                return "yesterday";
            }
            if (delta < 30 * DAY)
            {
                return ts.Days + " days ago";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        static public System.Windows.Forms.DialogResult ShowInputDialog(string title, ref string input)
        {
            var size = new System.Drawing.Size(280, 90);
            var inputBox = new System.Windows.Forms.Form();
            int top = 5;

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            var textLabel = new System.Windows.Forms.Label();
            textLabel.Size = new System.Drawing.Size(size.Width - 10, 23);
            textLabel.Location = new System.Drawing.Point(5, top);
            textLabel.Text = title;
            inputBox.Controls.Add(textLabel);
            top += textLabel.Height + 5;

            var textBox = new System.Windows.Forms.TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, top);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);
            top += textBox.Height + 5;

            var okButton = new System.Windows.Forms.Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, top);
            inputBox.Controls.Add(okButton);

            var cancelButton = new System.Windows.Forms.Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, top);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            var result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        static public bool IsCameyoFile(Stream contents)
        {
            // Check if this is a Cameyo package
            if (contents.Length < 1024 * 1024)
                return false;

            // Cameyo package
            byte[] eof = new byte[4];
            byte[] magic = { 0xBE, 0xBA, 0xCA, 0xDE };
            contents.Seek(-4, SeekOrigin.End);
            contents.Read(eof, 0, 4);
            bool match = true;
            for (int i = 0; i < 4; i++)
            {
                if (eof[i] != magic[i])
                {
                    match = false;
                    break;
                }
            }
            if (match)
                return true;

            // Usually, EndOfPeContainer = FileSize, but if there has been any addition to the file (ie digital certificate), 
            // then we need to scan it.
            // Max certificate size: 32kb
            int bufLen = 32 * 1024;
            byte[] buf = new byte[bufLen];
            contents.Seek(-bufLen, SeekOrigin.End);
            contents.Read(buf, 0, bufLen);
            for (int i = 0; i < bufLen - magic.Length; i++)
            {
                match = true;
                for (int j = 0; j < magic.Length; j++)
                {
                    if (buf[i + j] != magic[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Encrypts and decrypts data using DPAPI functions.
    /// </summary>
    public class DPAPI
    {
        // Wrapper for DPAPI CryptProtectData function.
        [DllImport("crypt32.dll",
                    SetLastError = true,
                    CharSet = CharSet.Auto)]
        private static extern
            bool CryptProtectData(ref DATA_BLOB pPlainText,
                                        string szDescription,
                                    ref DATA_BLOB pEntropy,
                                        IntPtr pReserved,
                                    ref CRYPTPROTECT_PROMPTSTRUCT pPrompt,
                                        int dwFlags,
                                    ref DATA_BLOB pCipherText);

        // Wrapper for DPAPI CryptUnprotectData function.
        [DllImport("crypt32.dll",
                    SetLastError = true,
                    CharSet = CharSet.Auto)]
        private static extern
            bool CryptUnprotectData(ref DATA_BLOB pCipherText,
                                    ref string pszDescription,
                                    ref DATA_BLOB pEntropy,
                                        IntPtr pReserved,
                                    ref CRYPTPROTECT_PROMPTSTRUCT pPrompt,
                                        int dwFlags,
                                    ref DATA_BLOB pPlainText);

        // BLOB structure used to pass data to DPAPI functions.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }

        // Prompt structure to be used for required parameters.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CRYPTPROTECT_PROMPTSTRUCT
        {
            public int cbSize;
            public int dwPromptFlags;
            public IntPtr hwndApp;
            public string szPrompt;
        }

        // Wrapper for the NULL handle or pointer.
        static private IntPtr NullPtr = ((IntPtr)((int)(0)));

        // DPAPI key initialization flags.
        private const int CRYPTPROTECT_UI_FORBIDDEN = 0x1;
        private const int CRYPTPROTECT_LOCAL_MACHINE = 0x4;

        /// <summary>
        /// Initializes empty prompt structure.
        /// </summary>
        /// <param name="ps">
        /// Prompt parameter (which we do not actually need).
        /// </param>
        private static void InitPrompt(ref CRYPTPROTECT_PROMPTSTRUCT ps)
        {
            ps.cbSize = Marshal.SizeOf(
                                      typeof(CRYPTPROTECT_PROMPTSTRUCT));
            ps.dwPromptFlags = 0;
            ps.hwndApp = NullPtr;
            ps.szPrompt = null;
        }

        /// <summary>
        /// Initializes a BLOB structure from a byte array.
        /// </summary>
        /// <param name="data">
        /// Original data in a byte array format.
        /// </param>
        /// <param name="blob">
        /// Returned blob structure.
        /// </param>
        private static void InitBLOB(byte[] data, ref DATA_BLOB blob)
        {
            // Use empty array for null parameter.
            if (data == null)
                data = new byte[0];

            // Allocate memory for the BLOB data.
            blob.pbData = Marshal.AllocHGlobal(data.Length);

            // Make sure that memory allocation was successful.
            if (blob.pbData == IntPtr.Zero)
                throw new Exception(
                    "Unable to allocate data buffer for BLOB structure.");

            // Specify number of bytes in the BLOB.
            blob.cbData = data.Length;

            // Copy data from original source to the BLOB structure.
            Marshal.Copy(data, 0, blob.pbData, data.Length);
        }

        // Flag indicating the type of key. DPAPI terminology refers to
        // key types as user store or machine store.
        public enum KeyType { UserKey = 1, MachineKey };

        // It is reasonable to set default key type to user key.
        private static KeyType defaultKeyType = KeyType.UserKey;

        /// <summary>
        /// Calls DPAPI CryptProtectData function to encrypt a plaintext
        /// string value with a user-specific key. This function does not
        /// specify data description and additional entropy.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext data to be encrypted.
        /// </param>
        /// <returns>
        /// Encrypted value in a base64-encoded format.
        /// </returns>
        public static string Encrypt(string plainText)
        {
            return Encrypt(defaultKeyType, plainText, String.Empty,
                            String.Empty);
        }

        /// <summary>
        /// Calls DPAPI CryptProtectData function to encrypt a plaintext
        /// string value. This function does not specify data description
        /// and additional entropy.
        /// </summary>
        /// <param name="keyType">
        /// Defines type of encryption key to use. When user key is
        /// specified, any application running under the same user account
        /// as the one making this call, will be able to decrypt data.
        /// Machine key will allow any application running on the same
        /// computer where data were encrypted to perform decryption.
        /// Note: If optional entropy is specifed, it will be required
        /// for decryption.
        /// </param>
        /// <param name="plainText">
        /// Plaintext data to be encrypted.
        /// </param>
        /// <returns>
        /// Encrypted value in a base64-encoded format.
        /// </returns>
        public static string Encrypt(KeyType keyType, string plainText)
        {
            return Encrypt(keyType, plainText, String.Empty,
                            String.Empty);
        }

        /// <summary>
        /// Calls DPAPI CryptProtectData function to encrypt a plaintext
        /// string value. This function does not specify data description.
        /// </summary>
        /// <param name="keyType">
        /// Defines type of encryption key to use. When user key is
        /// specified, any application running under the same user account
        /// as the one making this call, will be able to decrypt data.
        /// Machine key will allow any application running on the same
        /// computer where data were encrypted to perform decryption.
        /// Note: If optional entropy is specifed, it will be required
        /// for decryption.
        /// </param>
        /// <param name="plainText">
        /// Plaintext data to be encrypted.
        /// </param>
        /// <param name="entropy">
        /// Optional entropy which - if specified - will be required to
        /// perform decryption.
        /// </param>
        /// <returns>
        /// Encrypted value in a base64-encoded format.
        /// </returns>
        public static string Encrypt(KeyType keyType,
                                     string plainText,
                                     string entropy)
        {
            return Encrypt(keyType, plainText, entropy, String.Empty);
        }

        /// <summary>
        /// Calls DPAPI CryptProtectData function to encrypt a plaintext
        /// string value.
        /// </summary>
        /// <param name="keyType">
        /// Defines type of encryption key to use. When user key is
        /// specified, any application running under the same user account
        /// as the one making this call, will be able to decrypt data.
        /// Machine key will allow any application running on the same
        /// computer where data were encrypted to perform decryption.
        /// Note: If optional entropy is specifed, it will be required
        /// for decryption.
        /// </param>
        /// <param name="plainText">
        /// Plaintext data to be encrypted.
        /// </param>
        /// <param name="entropy">
        /// Optional entropy which - if specified - will be required to
        /// perform decryption.
        /// </param>
        /// <param name="description">
        /// Optional description of data to be encrypted. If this value is
        /// specified, it will be stored along with encrypted data and
        /// returned as a separate value during decryption.
        /// </param>
        /// <returns>
        /// Encrypted value in a base64-encoded format.
        /// </returns>
        public static string Encrypt(KeyType keyType,
                                     string plainText,
                                     string entropy,
                                     string description)
        {
            // Make sure that parameters are valid.
            if (plainText == null) plainText = String.Empty;
            if (entropy == null) entropy = String.Empty;

            // Call encryption routine and convert returned bytes into
            // a base64-encoded value.
            return Convert.ToBase64String(
                    Encrypt(keyType,
                            Encoding.UTF8.GetBytes(plainText),
                            Encoding.UTF8.GetBytes(entropy),
                            description));
        }

        /// <summary>
        /// Calls DPAPI CryptProtectData function to encrypt an array of
        /// plaintext bytes.
        /// </summary>
        /// <param name="keyType">
        /// Defines type of encryption key to use. When user key is
        /// specified, any application running under the same user account
        /// as the one making this call, will be able to decrypt data.
        /// Machine key will allow any application running on the same
        /// computer where data were encrypted to perform decryption.
        /// Note: If optional entropy is specifed, it will be required
        /// for decryption.
        /// </param>
        /// <param name="plainTextBytes">
        /// Plaintext data to be encrypted.
        /// </param>
        /// <param name="entropyBytes">
        /// Optional entropy which - if specified - will be required to
        /// perform decryption.
        /// </param>
        /// <param name="description">
        /// Optional description of data to be encrypted. If this value is
        /// specified, it will be stored along with encrypted data and
        /// returned as a separate value during decryption.
        /// </param>
        /// <returns>
        /// Encrypted value.
        /// </returns>
        public static byte[] Encrypt(KeyType keyType,
                                     byte[] plainTextBytes,
                                     byte[] entropyBytes,
                                     string description)
        {
            // Make sure that parameters are valid.
            if (plainTextBytes == null) plainTextBytes = new byte[0];
            if (entropyBytes == null) entropyBytes = new byte[0];
            if (description == null) description = String.Empty;

            // Create BLOBs to hold data.
            DATA_BLOB plainTextBlob = new DATA_BLOB();
            DATA_BLOB cipherTextBlob = new DATA_BLOB();
            DATA_BLOB entropyBlob = new DATA_BLOB();

            // We only need prompt structure because it is a required
            // parameter.
            CRYPTPROTECT_PROMPTSTRUCT prompt =
                                      new CRYPTPROTECT_PROMPTSTRUCT();
            InitPrompt(ref prompt);

            try
            {
                // Convert plaintext bytes into a BLOB structure.
                try
                {
                    InitBLOB(plainTextBytes, ref plainTextBlob);
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "Cannot initialize plaintext BLOB.", ex);
                }

                // Convert entropy bytes into a BLOB structure.
                try
                {
                    InitBLOB(entropyBytes, ref entropyBlob);
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "Cannot initialize entropy BLOB.", ex);
                }

                // Disable any types of UI.
                int flags = CRYPTPROTECT_UI_FORBIDDEN;

                // When using machine-specific key, set up machine flag.
                if (keyType == KeyType.MachineKey)
                    flags |= CRYPTPROTECT_LOCAL_MACHINE;

                // Call DPAPI to encrypt data.
                bool success = CryptProtectData(ref plainTextBlob,
                                                    description,
                                                ref entropyBlob,
                                                    IntPtr.Zero,
                                                ref prompt,
                                                    flags,
                                                ref cipherTextBlob);
                // Check the result.
                if (!success)
                {
                    // If operation failed, retrieve last Win32 error.
                    int errCode = Marshal.GetLastWin32Error();

                    // Win32Exception will contain error message corresponding
                    // to the Windows error code.
                    throw new Exception(
                        "CryptProtectData failed.", new Win32Exception(errCode));
                }

                // Allocate memory to hold ciphertext.
                byte[] cipherTextBytes = new byte[cipherTextBlob.cbData];

                // Copy ciphertext from the BLOB to a byte array.
                Marshal.Copy(cipherTextBlob.pbData,
                                cipherTextBytes,
                                0,
                                cipherTextBlob.cbData);

                // Return the result.
                return cipherTextBytes;
            }
            catch (Exception ex)
            {
                throw new Exception("DPAPI was unable to encrypt data.", ex);
            }
            // Free all memory allocated for BLOBs.
            finally
            {
                if (plainTextBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(plainTextBlob.pbData);

                if (cipherTextBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(cipherTextBlob.pbData);

                if (entropyBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(entropyBlob.pbData);
            }
        }

        /// <summary>
        /// Calls DPAPI CryptUnprotectData to decrypt ciphertext bytes.
        /// This function does not use additional entropy and does not
        /// return data description.
        /// </summary>
        /// <param name="cipherText">
        /// Encrypted data formatted as a base64-encoded string.
        /// </param>
        /// <returns>
        /// Decrypted data returned as a UTF-8 string.
        /// </returns>
        /// <remarks>
        /// When decrypting data, it is not necessary to specify which
        /// type of encryption key to use: user-specific or
        /// machine-specific; DPAPI will figure it out by looking at
        /// the signature of encrypted data.
        /// </remarks>
        public static string Decrypt(string cipherText)
        {
            string description;

            return Decrypt(cipherText, String.Empty, out description);
        }

        /// <summary>
        /// Calls DPAPI CryptUnprotectData to decrypt ciphertext bytes.
        /// This function does not use additional entropy.
        /// </summary>
        /// <param name="cipherText">
        /// Encrypted data formatted as a base64-encoded string.
        /// </param>
        /// <param name="description">
        /// Returned description of data specified during encryption.
        /// </param>
        /// <returns>
        /// Decrypted data returned as a UTF-8 string.
        /// </returns>
        /// <remarks>
        /// When decrypting data, it is not necessary to specify which
        /// type of encryption key to use: user-specific or
        /// machine-specific; DPAPI will figure it out by looking at
        /// the signature of encrypted data.
        /// </remarks>
        public static string Decrypt(string cipherText,
                                     out string description)
        {
            return Decrypt(cipherText, String.Empty, out description);
        }

        /// <summary>
        /// Calls DPAPI CryptUnprotectData to decrypt ciphertext bytes.
        /// </summary>
        /// <param name="cipherText">
        /// Encrypted data formatted as a base64-encoded string.
        /// </param>
        /// <param name="entropy">
        /// Optional entropy, which is required if it was specified during
        /// encryption.
        /// </param>
        /// <param name="description">
        /// Returned description of data specified during encryption.
        /// </param>
        /// <returns>
        /// Decrypted data returned as a UTF-8 string.
        /// </returns>
        /// <remarks>
        /// When decrypting data, it is not necessary to specify which
        /// type of encryption key to use: user-specific or
        /// machine-specific; DPAPI will figure it out by looking at
        /// the signature of encrypted data.
        /// </remarks>
        public static string Decrypt(string cipherText,
                                         string entropy,
                                     out string description)
        {
            // Make sure that parameters are valid.
            if (entropy == null) entropy = String.Empty;

            byte[] UnBase64 = Convert.FromBase64String(cipherText);

            return Encoding.UTF8.GetString(
                        Decrypt(UnBase64,
                                    Encoding.UTF8.GetBytes(entropy),
                                out description));
        }

        /// <summary>
        /// Calls DPAPI CryptUnprotectData to decrypt ciphertext bytes.
        /// </summary>
        /// <param name="cipherTextBytes">
        /// Encrypted data.
        /// </param>
        /// <param name="entropyBytes">
        /// Optional entropy, which is required if it was specified during
        /// encryption.
        /// </param>
        /// <param name="description">
        /// Returned description of data specified during encryption.
        /// </param>
        /// <returns>
        /// Decrypted data bytes.
        /// </returns>
        /// <remarks>
        /// When decrypting data, it is not necessary to specify which
        /// type of encryption key to use: user-specific or
        /// machine-specific; DPAPI will figure it out by looking at
        /// the signature of encrypted data.
        /// </remarks>
        public static byte[] Decrypt(byte[] cipherTextBytes,
                                         byte[] entropyBytes,
                                     out string description)
        {
            // Create BLOBs to hold data.
            DATA_BLOB plainTextBlob = new DATA_BLOB();
            DATA_BLOB cipherTextBlob = new DATA_BLOB();
            DATA_BLOB entropyBlob = new DATA_BLOB();

            // We only need prompt structure because it is a required
            // parameter.
            CRYPTPROTECT_PROMPTSTRUCT prompt =
                                      new CRYPTPROTECT_PROMPTSTRUCT();
            InitPrompt(ref prompt);

            // Initialize description string.
            description = String.Empty;

            try
            {
                // Convert ciphertext bytes into a BLOB structure.
                try
                {
                    InitBLOB(cipherTextBytes, ref cipherTextBlob);
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "Cannot initialize ciphertext BLOB.", ex);
                }

                // Convert entropy bytes into a BLOB structure.
                try
                {
                    InitBLOB(entropyBytes, ref entropyBlob);
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "Cannot initialize entropy BLOB.", ex);
                }

                // Disable any types of UI. CryptUnprotectData does not
                // mention CRYPTPROTECT_LOCAL_MACHINE flag in the list of
                // supported flags so we will not set it up.
                int flags = CRYPTPROTECT_UI_FORBIDDEN;

                // Call DPAPI to decrypt data.
                bool success = CryptUnprotectData(ref cipherTextBlob,
                                                  ref description,
                                                  ref entropyBlob,
                                                      IntPtr.Zero,
                                                  ref prompt,
                                                      flags,
                                                  ref plainTextBlob);

                // Check the result.
                if (!success)
                {
                    // If operation failed, retrieve last Win32 error.
                    int errCode = Marshal.GetLastWin32Error();

                    // Win32Exception will contain error message corresponding
                    // to the Windows error code.
                    throw new Exception(
                        "CryptUnprotectData failed.", new Win32Exception(errCode));
                }

                // Allocate memory to hold plaintext.
                byte[] plainTextBytes = new byte[plainTextBlob.cbData];

                // Copy ciphertext from the BLOB to a byte array.
                Marshal.Copy(plainTextBlob.pbData,
                             plainTextBytes,
                             0,
                             plainTextBlob.cbData);

                // Return the result.
                return plainTextBytes;
            }
            catch (Exception ex)
            {
                throw new Exception("DPAPI was unable to decrypt data.", ex);
            }
            // Free all memory allocated for BLOBs.
            finally
            {
                if (plainTextBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(plainTextBlob.pbData);

                if (cipherTextBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(cipherTextBlob.pbData);

                if (entropyBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(entropyBlob.pbData);
            }
        }
    }
}
