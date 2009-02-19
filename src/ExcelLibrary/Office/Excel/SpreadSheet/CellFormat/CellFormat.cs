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

        /// <summary>
        /// Default Format.
        /// </summary>
        public static readonly CellFormat General = new CellFormat(CellFormatType.General, "General");
        
        /// <summary>
        /// Format the DateTime with: "YYYY-MM-DD" e.g: 2009-02-18
        /// </summary>
        public static readonly CellFormat Date = new CellFormat(CellFormatType.Date, @"YYYY\-MM\-DD");
        
        /// <summary>
        /// Format the DateTime with: "HH:mm:ss" e.g: 14:45:00
        /// </summary>
        /// <example>Format the DateTime with: "HH:mm:ss" e.g: 14:45:00</example>
        public static readonly CellFormat Time = new CellFormat(CellFormatType.Time, "HH:mm:ss");
        
        /// <summary>
        /// Format the number with: "#,###.00000" e.g: 100.12345
        /// </summary>
        /// <example>Format the number with: "#,###.00000" e.g: 100.12345</example>
        public static readonly CellFormat Engineer = new CellFormat(CellFormatType.Scientific, "#,###.00000");
    }
}
