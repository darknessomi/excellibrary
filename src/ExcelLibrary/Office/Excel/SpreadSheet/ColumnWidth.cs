using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace ExcelLibrary.SpreadSheet
{
    public class ColumnWidth
    {
        internal Dictionary<Pair<UInt16, UInt16>, UInt16> columnWidth = new Dictionary<Pair<ushort, ushort>, ushort>();

        public UInt16 Default = 8 * 256;

        public UInt16 this[UInt16 colIndex]
        {
            get
            {
                Pair<UInt16, UInt16> range = FindColumnRange(colIndex);
                if (columnWidth.ContainsKey(range))
                {
                    return columnWidth[range];
                }
                else
                {
                    return Default;
                }
            }
            set
            {
                Pair<UInt16, UInt16> range = FindColumnRange(colIndex);
                columnWidth[range] = value;
            }
        }

        /// <summary>
        /// Get or set column width, the unit is 1/256 of the width of the zero character, using default font.
        /// </summary>
        /// <param name="firstColIndex">Index to first column in the range</param>
        /// <param name="lastColIndex">Index to last column in the range</param>
        /// <returns></returns>
        public UInt16 this[UInt16 firstColIndex, UInt16 lastColIndex]
        {
            get { return columnWidth[new Pair<ushort, ushort>(firstColIndex, lastColIndex)]; }
            set { columnWidth[new Pair<ushort, ushort>(firstColIndex, lastColIndex)] = value; }
        }

        private Pair<UInt16, UInt16> FindColumnRange(UInt16 colIndex)
        {
            foreach (Pair<UInt16, UInt16> range in columnWidth.Keys)
            {
                if (range.Left <= colIndex && colIndex <= range.Right)
                {
                    return range;
                }
            }
            return new Pair<UInt16, UInt16>(colIndex, colIndex);
        }

        public IEnumerator<KeyValuePair<Pair<UInt16, UInt16>, UInt16>> GetEnumerator()
        {
            return columnWidth.GetEnumerator();
        }
    }
}
