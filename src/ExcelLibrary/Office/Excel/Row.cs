using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.Office.Excel
{
    public class Row
    {
        Dictionary<int, Cell> Cells = new Dictionary<int, Cell>();

        public int FirstColIndex = int.MaxValue;
        public int LastColIndex = int.MinValue;

        public Cell GetCell(int colIndex)
        {
            if (Cells.ContainsKey(colIndex))
            {
                return Cells[colIndex];
            }
            return Cell.EmptyCell;
        }

        public void SetCell(int colIndex, Cell cell)
        {
            FirstColIndex = Math.Min(FirstColIndex, colIndex);
            LastColIndex = Math.Max(LastColIndex, colIndex);
            Cells[colIndex] = cell;
        }

        public Dictionary<int, Cell>.Enumerator GetEnumerator()
        {
            return Cells.GetEnumerator();
        }
    }
}
