using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.SpreadSheet;

namespace ExcelLibrary.BinaryFileFormat
{
    public class WorksheetDecoder
    {
        public static Worksheet Decode(Workbook book, Stream stream, SharedResource sharedResource)
        {
            Worksheet sheet = new Worksheet();
            sheet.Book = book;
            List<Record> records = ReadRecords(stream, out sheet.Drawing);
            sheet.Cells = PopulateCells(records, sharedResource);
            sheet.Book.Records.AddRange(records);
            return sheet;
        }

        private static List<Record> ReadRecords(Stream stream, out MSODRAWING drawingRecord)
        {
            List<Record> records = new List<Record>();
            drawingRecord = null;
            Record record = Record.Read(stream);
            Record last_record = record;
            Record last_formula_record = null;
            last_record.Decode();
            if (record is BOF && ((BOF)record).StreamType == StreamType.Worksheet)
            {
                while (record.Type != RecordType.EOF)
                {
                    if (record.Type == RecordType.CONTINUE)
                    {
                        last_record.ContinuedRecords.Add(record);
                    }
                    else
                    {
                        switch (record.Type)
                        {
                            case RecordType.STRING:
                                // jetcat_au: use last_formula_record instead of last_record
                                if (last_formula_record is FORMULA)
                                {
                                    record.Decode();
                                    (last_formula_record as FORMULA).StringRecord = record as STRING;
                                }
                                break;
                            case RecordType.MSODRAWING:
                                if (drawingRecord == null)
                                {
                                    drawingRecord = record as MSODRAWING;
                                    records.Add(record);
                                }
                                else
                                {
                                    drawingRecord.ContinuedRecords.Add(record);
                                }
                                break;
                            default:
                                records.Add(record);
                                break;
                        }
                        // jetcat_au: see 4.8 Array Formulas and Shared Formulas
                        if (record.Type == RecordType.FORMULA)
                        {
                            last_formula_record = record;
                        }
                        else if (record.Type != RecordType.SHRFMLA && record.Type != RecordType.ARRAY)
                        {
                            last_formula_record = null;
                        }
                        last_record = record;
                    }
                    record = Record.Read(stream);
                }
                records.Add(record);
            }
            return records;
        }

        private static CellCollection PopulateCells(List<Record> records, SharedResource sharedResource)
        {
            CellCollection cells = new CellCollection();
            cells.SharedResource = sharedResource;
            foreach (Record record in records)
            {
                record.Decode();
                switch (record.Type)
                {
                    //case RecordType.DIMENSIONS:
                    //    DIMENSIONS dimensions = record as DIMENSIONS;
                    //    cells.FirstRowIndex = dimensions.FirstRow;
                    //    cells.FirstColIndex = dimensions.FirstColumn;
                    //    cells.LastRowIndex = dimensions.LastRow-1;
                    //    cells.LastColIndex = dimensions.LastColumn-1;
                    //    break;
                    case RecordType.BOOLERR:
                        BOOLERR boolerr = record as BOOLERR;
                        cells.CreateCell(boolerr.RowIndex, boolerr.ColIndex, boolerr.GetValue(), boolerr.XFIndex);
                        break;
                    case RecordType.LABEL:
                        LABEL label = record as LABEL;
                        cells.CreateCell(label.RowIndex, label.ColIndex, label.Value, label.XFIndex);
                        break;
                    case RecordType.LABELSST:
                        LABELSST labelsst = record as LABELSST;
                        Cell cell = cells.CreateCell(labelsst.RowIndex, labelsst.ColIndex, sharedResource.GetStringFromSST(labelsst.SSTIndex), labelsst.XFIndex);
                        cell.Style.RichTextFormat = sharedResource.SharedStringTable.RichTextFormatting[labelsst.SSTIndex];
                        break;
                    case RecordType.NUMBER:
                        NUMBER number = record as NUMBER;
                        cells.CreateCell(number.RowIndex, number.ColIndex, number.Value, number.XFIndex);
                        break;
                    case RecordType.RK:
                        RK rk = record as RK;
                        cells.CreateCell(rk.RowIndex, rk.ColIndex, Record.DecodeRK(rk.Value), rk.XFIndex);
                        break;
                    case RecordType.MULRK:
                        MULRK mulrk = record as MULRK;
                        int row = mulrk.RowIndex;
                        for (int col = mulrk.FirstColIndex; col <= mulrk.LastColIndex; col++)
                        {
                            int index = col - mulrk.FirstColIndex;
                            object value = Record.DecodeRK(mulrk.RKList[index]);
                            int XFindex = mulrk.XFList[index];
                            cells.CreateCell(row, col, value, XFindex);
                        }
                        break;
                    case RecordType.FORMULA:
                        FORMULA formula = record as FORMULA;
                        cells.CreateCell(formula.RowIndex, formula.ColIndex, formula.DecodeResult(), formula.XFIndex);
                        break;
                }
            }
            return cells;
        }

        /*
         * Page 171 of the OpenOffice documentation of the Excel File Format
         * 
         * The font with index 4 is omitted in all BIFF versions. This means the first four fonts have zero-based indexes, 
         * and the fifth font and all following fonts are referenced with one-based indexes.
         */
        //public FONT getFontRecord(int index)
        private static FONT getFontRecord(SharedResource sharedResource, UInt16 index)
        {
            if (index >= 0 && index <= 3)
            {
                return sharedResource.Fonts[index];
            }
            else if (index >= 5)
            {
                return sharedResource.Fonts[index - 1];
            }
            else // index == 4 -> error
            {
                return null;
            }
        }

        /*
         * Sunil Shenoi, 8-25-2008
         * 
         * Assuming cell has a valid string vlaue, find the font record for a given characterIndex
         * into the stringValue of the cell
         */
        public static FONT getFontForCharacter(Cell cell, UInt16 charIndex)
        {
            FONT f = null;

            int index = cell.Style.RichTextFormat.CharIndexes.BinarySearch(charIndex);
            List<UInt16> fontIndexList = cell.Style.RichTextFormat.FontIndexes;
            
            if (index >= 0)
            {
                // found the object, return the font record
                f = getFontRecord(cell.SharedResource, fontIndexList[index]);
                //Console.WriteLine("for charIndex={0}, fontIndex={1})", charIndex, fontIndexList[index]);
                //Console.WriteLine("Object: {0} found at [{1}]", o, index);
            }
            else
            {
                // would have been inserted before the returned value, so insert just before it
                if (~index == 0)
                {
                    //f = getFontRecord(sheet,fontIndexList[0]);
                    //Console.WriteLine("for charIndex={0}, fontIndex=CELL", charIndex);
                }
                else
                {
                    f = getFontRecord(cell.SharedResource, fontIndexList[(~index) - 1]);
                    //Console.WriteLine("for charIndex={0}, fontIndex={1})", charIndex, fontIndexList[(~index) - 1]);
                }
                //Console.WriteLine("Object: {0} not found. "
                //   + "Next larger object found at [{1}].", o, ~index);
            }

            return f;
        }
    }
}
