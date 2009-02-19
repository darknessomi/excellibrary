using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.SpreadSheet;

namespace ExcelLibrary.BinaryFileFormat
{
    public class WorkbookDecoder
    {
        public static Workbook Decode(Stream stream)
        {
            Workbook book = new Workbook();
            SharedResource sharedResource;
            List<Record> records = ReadRecords(stream, out book.DrawingGroup);
            book.Records = records;
            List<BOUNDSHEET> boundSheets = DecodeRecords(records, out sharedResource);
            foreach (BOUNDSHEET boundSheet in boundSheets)
            {
                stream.Position = boundSheet.StreamPosition;
                Worksheet sheet = WorksheetDecoder.Decode(book, stream, sharedResource);
                sheet.Book = book;
                sheet.Name = boundSheet.SheetName;
                sheet.SheetType = (SheetType)boundSheet.SheetType;
                book.Worksheets.Add(sheet);
            }
            return book;
        }

        private static List<Record> ReadRecords(Stream stream, out MSODRAWINGGROUP drawingGroup)
        {
            List<Record> records = new List<Record>();
            drawingGroup = null;
            Record record = Record.Read(stream);
            record.Decode();
            Record last_record = record;
            if (record is BOF && ((BOF)record).StreamType == StreamType.WorkbookGlobals)
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
                            case RecordType.MSODRAWINGGROUP:
                                if (drawingGroup == null)
                                {
                                    drawingGroup = record as MSODRAWINGGROUP;
                                    records.Add(record);
                                }
                                else
                                {
                                    drawingGroup.ContinuedRecords.Add(record);
                                }
                                break;
                            default:
                                records.Add(record);
                                break;
                        }
                        last_record = record;
                    }
                    record = Record.Read(stream);
                }
                records.Add(record);
            }
            else
            {
                throw new Exception("Invalid Workbook.");
            }
            return records;
        }

        private static List<BOUNDSHEET> DecodeRecords(List<Record> records, out SharedResource sharedResource)
        {
            sharedResource = new SharedResource();
            List<BOUNDSHEET> boundSheets = new List<BOUNDSHEET>();
            foreach (Record record in records)
            {
                record.Decode();
                switch (record.Type)
                {
                    case RecordType.BOUNDSHEET:
                        boundSheets.Add(record as BOUNDSHEET);
                        break;
                    case RecordType.XF:
                        sharedResource.ExtendedFormats.Add(record as XF);
                        break;
                    case RecordType.FORMAT:
                        sharedResource.CellFormats.Add(record as FORMAT);
                        break;
                    case RecordType.SST:
                        sharedResource.SharedStringTable = record as SST;
                        break;
                    case RecordType.DATEMODE:
                        DATEMODE dateMode = record as DATEMODE;
                        switch (dateMode.Mode)
                        {
                            case 0:
                                sharedResource.BaseDate = DateTime.Parse("1899-12-31");
                                break;
                            case 1:
                                sharedResource.BaseDate = DateTime.Parse("1904-01-01");
                                break;
                        }
                        break;
                    case RecordType.PALETTE:
                        PALETTE palette = record as PALETTE;
                        int colorIndex = 8;
                        foreach (int color in palette.Colors)
                        {
                            sharedResource.ColorPalette[colorIndex] = Color.FromArgb(color);
                            colorIndex++;
                        }
                        break;
                    case RecordType.FONT:
                        FONT f = record as FONT;
                        sharedResource.Fonts.Add(f);
                        break;

                }
            }
            return boundSheets;
        }
    }
}
