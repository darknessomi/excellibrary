using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelLibrary.Office.Excel
{
    public class Cell
    {
        private object _value;

        internal int XFIndex;

        public string NumberFormat;

        internal SharedResource SharedResource;

        public static readonly Cell EmptyCell = new Cell(null, 0);

        public Cell(object value)
        {
            _value = value;
            NumberFormat = "GENERAL";
        }

        public Cell(object value, string numberFormat)
        {
            _value = value;
            NumberFormat = numberFormat;
        }

        public Cell(object value, int xfindex)
        {
            _value = value;
            XFIndex = xfindex;
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
                    if (days > 366) days--;
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
                double days = SharedResource.EncodeDateTime(value);
                this._value = days;
            }
        }

        public int BackColorIndex
        {
            get
            {
                return SharedResource.ExtendedFormats[XFIndex].PatternColorIndex;
            }
        }

        public Color BackColor
        {
            get
            {
                return SharedResource.ColorPalette[BackColorIndex];
            }
        }

        public ushort FormatIndex
        {
            get
            {
                return SharedResource.ExtendedFormats[XFIndex].FormatIndex;
            }
        }

        public CellFormat Format
        {
            get
            {
                return SharedResource.CellFormats[FormatIndex];
            }
        }
    }
}
