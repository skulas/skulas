using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dokka_excel_addin
{
    public class DokkaConfig
    {
        public static DokkaConfig shared = new DokkaConfig();

        // NOTE: This regexp doesn't allow spaces in the filename. Fix needed if requirements allow space in filename.
        private string _filenameRegexPattern = Properties.Settings.Default.DokkaFilenamePat;

        public string ServerRoot => Properties.Settings.Default.UploadServer;


        public bool IsDokkaManagedFilename(string filename)
        {
            // There's no pattern in the name, for now. Regex.IsMatch(filename, _filenameRegexPattern);
            // var isDokkaFilename = Regex.IsMatch(filename, _filenameRegexPattern);
            var isDokkaFilename = true; 

            return isDokkaFilename;
        }
    }
}
