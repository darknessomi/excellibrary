using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ExcelLibrary.BinaryFileFormat;

namespace ExcelLibrary.SpreadSheet
{
    public class Cell
    {
        private object _value;
        private CellFormat _format;
        private CellStyle _style;

        internal SharedResource SharedResource;

        public static readonly Cell EmptyCell = new Cell(null);

        public Cell(object value)
        {
            _value = value;
            _format = CellFormat.General;
        }

        public Cell(object value, string formatString)
        {
            _value = value;
            _format = new CellFormat(CellFormatType.General, formatString);
        }

        public Cell(object value, CellFormat format)
        {
            _value = value;
            _format = format;
        }

        public override string ToString()
        {
            return this.StringValue;
        }

        public bool IsEmpty
        {
            get { return this == EmptyCell; }
        }

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (IsEmpty) throw new Exception("Can not set value to an empty cell.");
                _value = value;
            }
        }

        public string StringValue
        {
            get
            {
                if (_value == null)
                {
                    return String.Empty;
                }
                else
                {
                    return _value.ToString();
                }
            }
        }

        public DateTime DateTimeValue
        {
            get
            {
                if (_value is double)
                {
                    double days = (double)_value;
                    //Excel counts an extra day for 1900-Feb-29. In reality, 1900 is not a leap year.
                    if (SharedResource.BaseDate == DateTime.Parse("1899-12-31") && days > 59)
                    {
                        days--;
                    }
                    return SharedResource.BaseDate.AddDays(days);
                }
                else if (_value is string)
                {
                    return DateTime.Parse((string)_value);
                }
                else if (_value is DateTime)
                {
                    return (DateTime)_value;
                }
                else
                {
                    throw new Exception("Invalid DateTime Cell.");
                }
            }
            set
            {
                this._value = value;
            }
        }

        public string FormatString
        {
            get { return _format.FormatString; }
            set { _format.FormatString = value; }
        }

        public CellFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }

        public CellStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        public FONT GetFontForCharacter(UInt16 charIndex)
        {
            return WorksheetDecoder.getFontForCharacter(this, charIndex);
        }
    }
}
