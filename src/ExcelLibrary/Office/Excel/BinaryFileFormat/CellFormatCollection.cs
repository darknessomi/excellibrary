using System;
using System.Collections.Generic;
using System.Text;
using ExcelLibrary.SpreadSheet;

namespace ExcelLibrary.BinaryFileFormat
{
    /// <summary>
    /// Excel built-in cell format 
    /// source:             
    ///  http://svn.apache.org/viewvc/poi/trunk/src/java/org/apache/poi/hssf/usermodel/HSSFDataFormat.java?view=markup  
    ///  http://sc.openoffice.org/excelfileformat.pdf 
    /// </summary>
    public class CellFormatCollection
    {
        private Dictionary<ushort, CellFormat> lookupTable;

        public CellFormatCollection()
        {
            lookupTable = new Dictionary<ushort, CellFormat>();
            lookupTable.Add(0, new CellFormat(CellFormatType.General, "General"));
            lookupTable.Add(1, new CellFormat(CellFormatType.Number, "0"));
            lookupTable.Add(2, new CellFormat(CellFormatType.Number, "0.00"));
            lookupTable.Add(3, new CellFormat(CellFormatType.Number, "#,##0"));
            lookupTable.Add(4, new CellFormat(CellFormatType.Number, "#,##0.00"));
            lookupTable.Add(5, new CellFormat(CellFormatType.Currency, "($#,##0_);($#,##0)"));
            lookupTable.Add(6, new CellFormat(CellFormatType.Currency, "($#,##0_);[Red]($#,##0)"));
            lookupTable.Add(7, new CellFormat(CellFormatType.Currency, "($#,##0.00);($#,##0.00)"));
            lookupTable.Add(8, new CellFormat(CellFormatType.Currency, "($#,##0.00_);[Red]($#,##0.00)"));
            lookupTable.Add(9, new CellFormat(CellFormatType.Percentage, "0%"));
            lookupTable.Add(10, new CellFormat(CellFormatType.Percentage, "0.00%"));
            lookupTable.Add(11, new CellFormat(CellFormatType.Scientific, "0.00E+00"));
            lookupTable.Add(12, new CellFormat(CellFormatType.Fraction, "# ?/?"));
            lookupTable.Add(13, new CellFormat(CellFormatType.Fraction, "# ??/??"));
            lookupTable.Add(14, new CellFormat(CellFormatType.Date, "m/d/yy"));
            lookupTable.Add(15, new CellFormat(CellFormatType.Date, "d-mmm-yy"));
            lookupTable.Add(16, new CellFormat(CellFormatType.Date, "d-mmm"));
            lookupTable.Add(17, new CellFormat(CellFormatType.Date, "mmm-yy"));
            lookupTable.Add(18, new CellFormat(CellFormatType.Time, "h:mm AM/PM"));
            lookupTable.Add(19, new CellFormat(CellFormatType.Time, "h:mm:ss AM/PM"));
            lookupTable.Add(20, new CellFormat(CellFormatType.Time, "h:mm"));
            lookupTable.Add(21, new CellFormat(CellFormatType.Time, "h:mm:ss"));
            lookupTable.Add(22, new CellFormat(CellFormatType.DateTime, "m/d/yy h:mm"));
            lookupTable.Add(37, new CellFormat(CellFormatType.Accounting, "(#,##0_);(#,##0)"));
            lookupTable.Add(38, new CellFormat(CellFormatType.Accounting, "(#,##0_);[Red](#,##0)"));
            lookupTable.Add(39, new CellFormat(CellFormatType.Accounting, "(#,##0.00_);(#,##0.00)"));
            lookupTable.Add(40, new CellFormat(CellFormatType.Accounting, "(#,##0.00_);[Red](#,##0.00)"));
            lookupTable.Add(41, new CellFormat(CellFormatType.Currency, "_(*#,##0_);_(*(#,##0);_(* \"-\"_);_(@_)"));
            lookupTable.Add(42, new CellFormat(CellFormatType.Currency, "_($*#,##0_);_($*(#,##0);_($* \"-\"_);_(@_)"));
            lookupTable.Add(43, new CellFormat(CellFormatType.Currency, "_(*#,##0.00_);_(*(#,##0.00);_(*\"-\"??_);_(@_)"));
            lookupTable.Add(44, new CellFormat(CellFormatType.Currency, "_($*#,##0.00_);_($*(#,##0.00);_($*\"-\"??_);_(@_)"));
            lookupTable.Add(45, new CellFormat(CellFormatType.Time, "mm:ss"));
            lookupTable.Add(46, new CellFormat(CellFormatType.Time, "[h]:mm:ss"));
            lookupTable.Add(47, new CellFormat(CellFormatType.Time, "mm:ss.0"));
            lookupTable.Add(48, new CellFormat(CellFormatType.Scientific, "##0.0E+0"));
            lookupTable.Add(49, new CellFormat(CellFormatType.Text, "@"));
        }

        public void Add(FORMAT record)
        {
            if (record == null) return;
            // Built-in cell formula may change due to regional settings            
            // therefore, we allow caller to replace built-in cell format      
            if (this.lookupTable.ContainsKey(record.FormatIndex))
            {
                CellFormat oldCellFormat = this.lookupTable[record.FormatIndex];
                this.lookupTable[record.FormatIndex] = new CellFormat(oldCellFormat.FormatType, record.FormatString);
            }
            else
            {
                this.lookupTable.Add(record.FormatIndex, new CellFormat(CellFormatType.Custom, record.FormatString));
            }
        }

        public CellFormat this[ushort formatIndex]
        {
            get
            {
                if (this.lookupTable.ContainsKey(formatIndex))
                {
                    return this.lookupTable[formatIndex];
                }
                else
                {
                    throw new KeyNotFoundException("Unable to find specific cell format");
                }
            }
        }

        public UInt16 GetFormatIndex(string formatString)
        {
            foreach (KeyValuePair<UInt16, CellFormat> cellFormat in lookupTable)
            {
                if (formatString == cellFormat.Value.FormatString)
                {
                    return cellFormat.Key;
                }
            }
            return UInt16.MaxValue;
        }
    }
}



