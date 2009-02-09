using System;
using System.Collections.Generic;
using System.Text;
using ExcelLibrary.CodeLib;

namespace ExcelLibrary.Office.Excel
{
    public class WorkSheetEncoder
    {
        public static List<Record> Encode(Worksheet worksheet, SharedResource sharedResource)
        {
            List<Record> records = new List<Record>();
            BOF bof = new BOF();
            bof.BIFFversion = 0x0600; //0600H = BIFF8
            bof.StreamType = StreamType.Worksheet;
            bof.BuildID = 3515;
            bof.BuildYear = 1996;
            bof.RequiredExcelVersion = 6;
            records.Add(bof);

            foreach (KeyValuePair<Pair<UInt16, UInt16>, UInt16> colWidth in worksheet.Cells.ColumnWidth)
            {
                COLINFO colInfo = new COLINFO();
                colInfo.FirstColIndex = colWidth.Key.Left;
                colInfo.LastColIndex = colWidth.Key.Right;
                colInfo.Width = colWidth.Value;
                records.Add(colInfo);
            }

            DIMENSIONS dimensions = new DIMENSIONS();
            if (worksheet.Cells.Rows.Count > 0)
            {
                dimensions.FirstRow = worksheet.Cells.FirstRowIndex;
                dimensions.FirstColumn = (Int16)worksheet.Cells.FirstColIndex;
                dimensions.LastRow = worksheet.Cells.LastRowIndex + 1;
                dimensions.LastColumn = (Int16)(worksheet.Cells.LastColIndex + 1);
            }
            records.Add(dimensions);

            // each Row Block contains 32 consecutive rows
            List<Record> rowblock = new List<Record>(32);
            List<Record> cellblock = new List<Record>();
            for (int rowIndex = dimensions.FirstRow; rowIndex < dimensions.LastRow; rowIndex++)
            {
                if (worksheet.Cells.Rows.ContainsKey(rowIndex))
                {
                    Row sheetRow = worksheet.Cells.Rows[rowIndex];

                    ROW biffRow = new ROW();
                    biffRow.RowIndex = (UInt16)rowIndex;
                    biffRow.FirstColIndex = (UInt16)sheetRow.FirstColIndex;
                    biffRow.LastColIndex = (UInt16)(sheetRow.LastColIndex + 1);
                    biffRow.RowHeight = 0x0101; // default height 0x0080
                    biffRow.Flags = 0x0F0100; // defaul value 0x0100
                    rowblock.Add(biffRow);

                    for (int colIndex = sheetRow.FirstColIndex; colIndex <= sheetRow.LastColIndex; colIndex++)
                    {
                        Cell cell = sheetRow.GetCell(colIndex);
                        if (cell != Cell.EmptyCell && cell.Value != null)
                        {
                            CellValue cellRecord = EncodeCell(cell, sharedResource);
                            cellRecord.RowIndex = (UInt16)rowIndex;
                            cellRecord.ColIndex = (UInt16)colIndex;
                            cellRecord.XFIndex = (UInt16)sharedResource.GetXFIndex(cell.NumberFormat);
                            cellblock.Add(cellRecord);
                        }
                    }

                    if (rowblock.Count == 32)
                    {
                        records.AddRange(rowblock);
                        records.AddRange(cellblock);

                        rowblock.Clear();
                        cellblock.Clear();
                    }
                }
            }

            if (rowblock.Count > 0)
            {
                records.AddRange(rowblock);
                records.AddRange(cellblock);
            }

            EOF eof = new EOF();
            records.Add(eof);
            return records;
        }

        private static CellValue EncodeCell(Cell cell, SharedResource sharedResource)
        {
            object value = cell.Value;
            if (value is int)
            {
                RK rk = new RK();
                rk.Value = (uint)((int)value << 2 | 2);
                return rk;
            }
            else if (value is decimal)
            {
                RK rk = new RK();
                rk.Value = (uint)((decimal)value * 100) << 2 | 3; // integer and mul
                return rk;
            }
            else if (value is double)
            {
                //RK rk = new RK();
                //Int64 data = BitConverter.DoubleToInt64Bits((double)value);
                //rk.Value = (uint)(data >> 32) & 0xFFFFFFFC;
                //return rk;
                NUMBER number = new NUMBER();
                number.Value = (double)value;
                return number;
            }
            else if (value is string)
            {
                LABELSST label = new LABELSST();
                label.SSTIndex = sharedResource.GetSSTIndex((string)value);
                return label;
            }
            else if (value is DateTime)
            {
                NUMBER number = new NUMBER();
                number.Value = sharedResource.EncodeDateTime((DateTime)value);
                return number;
            }
            else if (value is bool)
            {
                BOOLERR boolerr = new BOOLERR();
                boolerr.ValueType = 0;
                boolerr.Value = Convert.ToByte((bool)value);
                return boolerr;
            }
            else if (value is ErrorCode)
            {
                BOOLERR boolerr = new BOOLERR();
                boolerr.ValueType = 1;
                boolerr.Value = ((ErrorCode)value).Code;
                return boolerr;
            }
            else
            {
                throw new Exception("Invalid cell value.");
            }
        }
    }
}
