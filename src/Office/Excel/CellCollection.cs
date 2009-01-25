using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace QiHe.Office.Excel
{
    public class CellCollection
    {
        public Dictionary<int, Row> Rows = new Dictionary<int, Row>();

        public int FirstRowIndex = int.MaxValue;
        public int FirstColIndex = int.MaxValue;
        public int LastRowIndex = 0;
        public int LastColIndex = 0;

        internal SharedResource SharedResource;

        internal ColumnWidth ColumnWidth = new ColumnWidth();

        public Cell CreateCell(int row, int col, object value, int XFindex)
        {
            Cell cell = new Cell(value, XFindex);
            this[row, col] = cell;
            return cell;
        }

        public Row GetRow(int rowIndex)
        {
            if (!Rows.ContainsKey(rowIndex))
            {
                Rows[rowIndex] = new Row();
            }
            return Rows[rowIndex];
        }

        /// <summary>
        /// Get cell by row and col index
        /// </summary>
        /// <param name="row">starts from 0.</param>
        /// <param name="col">starts from 0.</param>
        /// <returns></returns>
        public Cell this[int row, int col]
        {
            get
            {
                if (Rows.ContainsKey(row))
                {
                    return GetRow(row).GetCell(col);
                }
                return Cell.EmptyCell;
            }
            set
            {
                FirstRowIndex = Math.Min(FirstRowIndex, row);
                FirstColIndex = Math.Min(FirstColIndex, col);
                LastRowIndex = Math.Max(LastRowIndex, row);
                LastColIndex = Math.Max(LastColIndex, col);
                value.SharedResource = this.SharedResource;
                GetRow(row).SetCell(col, value);
            }
        }

        public IEnumerator<Pair<Pair<int, int>, Cell>> GetEnumerator()
        {
            foreach (KeyValuePair<int, Row> row in Rows)
            {
                foreach (KeyValuePair<int, Cell> cell in row.Value)
                {
                    yield return new Pair<Pair<int, int>, Cell>
                        (new Pair<int, int>(row.Key, cell.Key), cell.Value);
                }
            }
        }
    }
}
