using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.Office.Excel
{
    public class CellFormat
    {
        private CellFormatType formatType;
        private string formatString;

        public CellFormat(CellFormatType type, string fmt)
        {
            formatType = type;
            formatString = fmt;
        }

        public CellFormatType FormatType
        {
            get { return formatType; }
        }

        public string FormatString
        {
            get { return formatString; }
        }
    }
}
