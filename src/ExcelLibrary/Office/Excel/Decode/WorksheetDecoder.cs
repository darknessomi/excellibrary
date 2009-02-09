using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using ExcelLibrary.Office.CompoundDocumentFormat;

namespace ExcelLibrary.Office.Excel
{
    public class WorksheetDecoder
    {
        public static Worksheet Decode(Workbook book, Stream stream, SharedResource sharedResource)
        {
            Worksheet sheet = new Worksheet();
            sheet.Book = book;
            //MSODRAWING drawing;
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
                    case RecordType.LABELSST:
                        LABELSST label = record as LABELSST;
                        cells.CreateCell(label.RowIndex, label.ColIndex, sharedResource.GetStringFromSST(label.SSTIndex), label.XFIndex);
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
    }
}
