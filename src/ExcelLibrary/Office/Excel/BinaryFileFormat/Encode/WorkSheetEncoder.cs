using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;
using ExcelLibrary.BinaryDrawingFormat;
using ExcelLibrary.SpreadSheet;

namespace ExcelLibrary.BinaryFileFormat
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
                    biffRow.RowHeight = sheetRow.Height;
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
                            cellRecord.XFIndex = (UInt16)sharedResource.GetXFIndex(cell.Format);
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

            if (worksheet.Pictures.Count > 0)
            {
                records.Add(EncodePictures(worksheet.Pictures, sharedResource, worksheet));
                for (ushort id = 1; id <= worksheet.Pictures.Count; id++)
                {
                    OBJ obj = new OBJ();
                    CommonObjectData objData = new CommonObjectData();
                    objData.ObjectID = id;
                    objData.ObjectType = 8;
                    objData.OptionFlags = 24593;
                    obj.SubRecords.Add(objData);
                    obj.SubRecords.Add(new End());
                    records.Add(obj);
                }
            }

            EOF eof = new EOF();
            records.Add(eof);
            return records;
        }

        private static CellValue EncodeCell(Cell cell, SharedResource sharedResource)
        {
            object value = cell.Value;
            if (value is int || value is short || value is uint || value is byte)
            {
                RK rk = new RK();
                rk.Value = (uint)(Convert.ToInt32(value) << 2 | 2);
                return rk;
            }
            else if (value is decimal)
            {
                if (Math.Abs((decimal)value) <= (decimal)5368709.11)
                {
                    RK rk = new RK();
                    rk.Value = (uint)((int)((decimal)value * 100) << 2 | 3); // integer and mul
                    return rk;
                }
                else
                {
                    NUMBER number = new NUMBER();
                    number.Value = (double)(decimal)value;
                    return number;
                }
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

        private static Record EncodePictures(Dictionary<Pair<int, int>, Picture> pictures, SharedResource sharedResource, Worksheet worksheet)
        {
            MSODRAWING msoDrawing = new MSODRAWING();
            MsofbtDgContainer dgContainer = new MsofbtDgContainer();
            msoDrawing.EscherRecords.Add(dgContainer);

            MsofbtDg dg = new MsofbtDg();
            dg.Instance = 1;
            dg.NumShapes = pictures.Count + 1;
            dg.LastShapeID = 1024 + pictures.Count;
            dgContainer.EscherRecords.Add(dg);

            MsofbtSpgrContainer spgrContainer = new MsofbtSpgrContainer();
            dgContainer.EscherRecords.Add(spgrContainer);

            MsofbtSpContainer spContainer0 = new MsofbtSpContainer();
            spContainer0.EscherRecords.Add(new MsofbtSpgr());
            MsofbtSp shape0 = new MsofbtSp();
            shape0.ShapeId = 1024;
            shape0.Flags = ShapeFlag.Group | ShapeFlag.Patriarch;
            shape0.Version = 2;
            spContainer0.EscherRecords.Add(shape0);
            spgrContainer.EscherRecords.Add(spContainer0);

            foreach (Picture pic in pictures.Values)
            {
                if (!sharedResource.Images.Contains(pic.Image))
                {
                    sharedResource.Images.Add(pic.Image);
                }
                MsofbtSpContainer spContainer = new MsofbtSpContainer();
                MsofbtSp shape = new MsofbtSp();
                shape.Version = 2;
                shape.ShapeType = ShapeType.PictureFrame;
                shape.ShapeId = 1024 + spgrContainer.EscherRecords.Count;
                shape.Flags = ShapeFlag.Haveanchor | ShapeFlag.Hasshapetype;
                spContainer.EscherRecords.Add(shape);

                MsofbtOPT opt = new MsofbtOPT();
                opt.Add(PropertyIDs.LockAgainstGrouping, 33226880);
                opt.Add(PropertyIDs.FitTextToShape, 262148);
                opt.Add(PropertyIDs.BlipId, (uint)sharedResource.Images.IndexOf(pic.Image) + 1);
                spContainer.EscherRecords.Add(opt);

                MsofbtClientAnchor anchor = new MsofbtClientAnchor();
                anchor.Row1 = pic.TopLeftCorner.RowIndex;
                anchor.Col1 = pic.TopLeftCorner.ColIndex;
                anchor.DX1 = pic.TopLeftCorner.DX;
                anchor.DY1 = pic.TopLeftCorner.DY;
                anchor.Row2 = pic.BottomRightCorner.RowIndex;
                anchor.Col2 = pic.BottomRightCorner.ColIndex;
                anchor.DX2 = pic.BottomRightCorner.DX;
                anchor.DY2 = pic.BottomRightCorner.DY;
                anchor.ExtraData = new byte[0];
                spContainer.EscherRecords.Add(anchor);

                spContainer.EscherRecords.Add(new MsofbtClientData());

                spgrContainer.EscherRecords.Add(spContainer);
            }
            return msoDrawing;
        }
    }
}
