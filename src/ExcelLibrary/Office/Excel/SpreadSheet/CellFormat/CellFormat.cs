using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.SpreadSheet
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
            set { formatString = value; }
        }

        public static readonly CellFormat General = new CellFormat(CellFormatType.General, "General");
        public static readonly CellFormat Date = new CellFormat(CellFormatType.Date, @"YYYY\-MM\-DD");
    }
}
