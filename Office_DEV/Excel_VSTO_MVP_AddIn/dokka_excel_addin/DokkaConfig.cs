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
        private const string DOKKA_PREFIX = "Dokka#managed#file";
        private string _filenameRegexPattern = $"^{DOKKA_PREFIX}_.*_{"[a-fA-F0-9]{10}"}_.*\\..*";

        private const string CLIENT_PATH_PARAM = "3"; // 1 - iOS, 2 - Android, 3 - Web
        public string ServerRoot => $"http://api-dev.dokka.biz/api/v2/{CLIENT_PATH_PARAM}";


        public bool IsDokkaManagedFilename(string filename)
        {
            var isDokkaFilename = Regex.IsMatch(filename, _filenameRegexPattern);

            return isDokkaFilename;
        }
    }
}
