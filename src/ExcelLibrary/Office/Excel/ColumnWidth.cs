using System;
using System.Collections.Generic;
using System.Text;
using ExcelLibrary.CodeLib;

namespace ExcelLibrary.Office.Excel
{
    public class ColumnWidth
    {
        internal Dictionary<Pair<UInt16, UInt16>, UInt16> columnWidth = new Dictionary<Pair<ushort, ushort>, ushort>();

        public UInt16 this[UInt16 colIndex]
        {
            get { return columnWidth[new Pair<ushort, ushort>(colIndex, colIndex)]; }
            set { columnWidth[new Pair<ushort, ushort>(colIndex, colIndex)] = value; }
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

        public IEnumerator<KeyValuePair<Pair<UInt16, UInt16>, UInt16>> GetEnumerator()
        {
            return columnWidth.GetEnumerator();
        }
    }
}
