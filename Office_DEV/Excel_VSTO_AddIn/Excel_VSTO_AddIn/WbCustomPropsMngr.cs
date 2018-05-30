using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_VSTO_AddIn
{
    // This class shouldn't be singleton.
    // Yet, since meta data can become more useful, I decided to make it this way
    // for future scalability issues such as statefull document properties.

    public class WbCustomPropsMngr
    {

        #region Life Cycle

        public static WbCustomPropsMngr Instance { get; private set; } = new WbCustomPropsMngr();

        private static readonly string DOKKA_DOCUMENT_ID_KEY = "Dokka Document ID";
        public static readonly string DOKKA_DEFAULT_DOCUMENT_ID = "Dokka Default Document ID";

        private WbCustomPropsMngr()
        {
            // NOP - just block default constructor.
        }

        #endregion Life Cycle



        #region Public Funcs

        public void SetDocumentIdForWb(Workbook Wb, string documentId)
        {
            if (Wb == null)
            {
                Trace.TraceError("Trying to set Custom Props to a null Wb");
                return;
            }

            if (String.IsNullOrEmpty(documentId))
            {
                Trace.WriteLine("Removing Document Id from Wb");
                DeleteWbProperty(Wb, DOKKA_DOCUMENT_ID_KEY);
            } else
            {
                SetWbProperty(Wb, DOKKA_DOCUMENT_ID_KEY, documentId);
            }
        }

        public string GetDoucmentIdForWb(Workbook Wb)
        {
            string docId = null;
            var prop = GetCustomProperty(Wb, DOKKA_DOCUMENT_ID_KEY);

            if (prop != null)
            {
                docId = prop.Value;
            }

            return docId;
        }

        #endregion Public Funcs



        #region Private

        private Microsoft.Office.Core.DocumentProperty GetCustomProperty(Workbook Wb, string KEY)
        {
            Microsoft.Office.Core.DocumentProperty result = null;
            Microsoft.Office.Core.DocumentProperties customProps = Wb.CustomDocumentProperties;
            try
            {
                result = customProps[KEY];
            }
            catch { /* NOP - DON'T CARE IF DOESN'T EXIST */}

            return result;
        }

        /// <summary>
        /// Sets any CUSTOM property to a given Wb.
        /// </summary>
        /// <remarks>
        /// No data validation done, send legal data only to this function.
        /// </remarks>
        /// <param name="KEY"></param>
        /// <param name="value"></param>
        private void SetWbProperty(Workbook Wb, string KEY, string value)
        {
            Microsoft.Office.Core.DocumentProperties customProps = Wb.CustomDocumentProperties;

            // Clear Current Value
            DeleteWbProperty(Wb, KEY);

            customProps.Add(KEY, false, Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString, value);
        }

        private void DeleteWbProperty(Workbook Wb, string KEY)
        {
            var currProp = GetCustomProperty(Wb, KEY);
            if (currProp != null) currProp.Delete();
        }

        #endregion Private

    }
}
