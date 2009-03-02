using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;
using ExcelLibrary.BinaryFileFormat;

namespace ExcelLibrary.SpreadSheet
{
    public class CellCollection
    {
        public Dictionary<int, Row> Rows = new Dictionary<int, Row>();

        public int FirstRowIndex = int.MaxValue;
        public int FirstColIndex = int.MaxValue;
        public int LastRowIndex = 0;
        public int LastColIndex = 0;

        internal SharedResource SharedResource;

        public ColumnWidth ColumnWidth = new ColumnWidth();

        public Cell CreateCell(int row, int col, object value, int XFindex)
        {
            XF xf = SharedResource.ExtendedFormats[XFindex];
            CellFormat foramt = SharedResource.CellFormats[xf.FormatIndex];
            Cell cell = new Cell(value, foramt);
            cell.SharedResource = this.SharedResource;
            cell.Style = CreateStyleFromXF(xf);
            this[row, col] = cell;
            return cell;
        }

        private CellStyle CreateStyleFromXF(XF xf)
        {
            CellStyle style = new CellStyle();
            style.BackColor = SharedResource.ColorPalette[xf.PatternColorIndex];
            return style;
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

        public UInt16 DefaultRowHeight = 300; // twips

        // experiment, not works correctly
        public UInt16 GetRowIndexByPos(int y, out UInt16 dy)
        {
            UInt16 rowIndex = 0;
            UInt16 height = 0;
            int pos = (int)(y * 15);
            dy = (UInt16)pos;
            while (true)
            {
                height += GetRowHeight(rowIndex);
                if (height <= pos)
                {
                    dy = (UInt16)(pos - height);
                    rowIndex++;
                }
                else
                {
                    break;
                }
            }
            return rowIndex;
        }

        public UInt16 GetRowHeight(int rowIndex)
        {
            if (Rows.ContainsKey(rowIndex))
            {
                return Rows[rowIndex].Height;
            }
            else
            {
                return DefaultRowHeight;
            }
        }

        // experiment, not works correctly
        public UInt16 GetColumnIndexByPos(int x, out UInt16 dx)
        {
            UInt16 colIndex = 0;
            UInt16 width = 0;
            int pos = (int)(x * 33.75);
            dx = (UInt16)pos;
            while (true)
            {
                width += ColumnWidth[colIndex];
                if (width <= pos)
                {
                    dx = (UInt16)(pos - width);
                    colIndex++;
                }
                else
                {
                    break;
                }
            }
            return colIndex;
        }
    }
}
