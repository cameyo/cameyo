/*
 * A collection of general purpose functions not belonging to any specific CameyoPackageEditor specific functions.
 * But do help these functions do common 'simple' tasks..
 */

using System;
using System.IO;

namespace PackageEditor.Utilities
{
  class Common
  {
    /*
     * Creates a unique folder in the users Temp
     * 
     * For example:  "C:\Users\USERNAME\Temp\BASEnameAsYouLikeIt_esartheh.sag\"
     */
    static public String CreateTempFolder(String baseName)
    {
      String result = null;
      String tempFolder = Path.GetTempPath();
      do
      {
        String newName = Path.Combine(tempFolder, baseName + Path.GetRandomFileName());
        if (!Directory.Exists(newName))
        {
          DirectoryInfo di = Directory.CreateDirectory(newName);
          result = di.FullName;
        }
      }
      while (result == null);
      return result;
    }
  }
}
