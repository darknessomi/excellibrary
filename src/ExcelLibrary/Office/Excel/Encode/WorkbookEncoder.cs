using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using ExcelLibrary.Office.CompoundDocumentFormat;
using ExcelLibrary.CodeLib;

namespace ExcelLibrary.Office.Excel
{
    public class WorkbookEncoder
    {
        public static void Encode(Workbook workbook, Stream stream)
        {
            List<Record> records = EncodeWorkbook(workbook);

            BinaryWriter writer = new BinaryWriter(stream);
            foreach (Record record in records)
            {
                record.Write(writer);
            }
            writer.Close();
        }

        private static List<Record> EncodeWorkbook(Workbook workbook)
        {
            SharedResource sharedResource = new SharedResource(true);
            List<Record> book_records = new List<Record>();
            BOF bof = new BOF();
            bof.BIFFversion = 0x0600; //0600H = BIFF8
            bof.StreamType = StreamType.WorkbookGlobals;
            bof.BuildID = 3515;
            bof.BuildYear = 1996;
            bof.RequiredExcelVersion = 6;
            book_records.Add(bof);

            CODEPAGE codepage = new CODEPAGE();
            codepage.CodePageIdentifier = (ushort)Encoding.Unicode.CodePage;
            book_records.Add(codepage);

            WINDOW1 window = new WINDOW1();
            window.WindowWidth = 16384;
            window.WindowHeight = 8192;
            window.SelecteWorksheets = 1;
            window.TabBarWidth = 600;
            window.OptionFlags = 56;
            book_records.Add(window);

            DATEMODE dateMode = new DATEMODE();
            dateMode.Mode = 1;
            sharedResource.BaseDate = DateTime.Parse("1904-01-01");
            book_records.Add(dateMode);

            List<List<Record>> all_sheet_records = new List<List<Record>>();
            foreach (Worksheet worksheet in workbook.Worksheets)
            {
                List<Record> sheet_records = WorkSheetEncoder.Encode(worksheet, sharedResource);
                Record.EncodeRecords(sheet_records);
                all_sheet_records.Add(sheet_records);
            }

            book_records.AddRange(sharedResource.FormatRecords.ToArray());
            book_records.AddRange(sharedResource.ExtendedFormats.ToArray());

            List<BOUNDSHEET> boundSheets = new List<BOUNDSHEET>();
            foreach (Worksheet worksheet in workbook.Worksheets)
            {
                BOUNDSHEET boundSheet = new BOUNDSHEET();
                boundSheet.Visibility = 0; // 00H = Visible
                boundSheet.SheetType = (byte)SheetType.Worksheet;
                boundSheet.SheetName = worksheet.Name;
                boundSheet.StreamPosition = 0;
                boundSheets.Add(boundSheet);
                book_records.Add(boundSheet);
            }

            Record.EncodeRecords(book_records);
            int sstOffset = Record.CountDataLength(book_records);

            book_records.Add(sharedResource.SharedStringTable);
            book_records.Add(CreateEXTSST(sharedResource.SharedStringTable, sstOffset));

            EOF eof = new EOF();
            book_records.Add(eof);

            Record.EncodeRecords(book_records);
            int dataLength = Record.CountDataLength(book_records);

            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                boundSheets[i].StreamPosition = (uint)dataLength;
                boundSheets[i].Encode();

                int sheet_length = Record.CountDataLength(all_sheet_records[0]);
                dataLength += sheet_length;
            }
            
            List<Record> all_records = new List<Record>();
            all_records.AddRange(book_records);
            foreach (List<Record> sheet_records in all_sheet_records)
            {
                all_records.AddRange(sheet_records);
            }
            return all_records;
        }

        private static EXTSST CreateEXTSST(SST sst, int sstOffset)
        {
            EXTSST extSST = new EXTSST();
            extSST.NumStrings = 8;

            int counter = 0;
            int totalLength = sstOffset + 0x0C;
            int relativeLength = 0x0C;
            foreach (string text in sst.StringList)
            {
                int stringLength = Record.GetStringDataLength(text);
                if (relativeLength + stringLength > Record.MaxContentLength + 4)
                {
                    totalLength += 4;
                    relativeLength = 4;
                }
                if (counter == 0)
                {
                    StringOffset stringOffset = new StringOffset();
                    stringOffset.AbsolutePosition = (uint)totalLength;
                    stringOffset.RelativePosition = (ushort)relativeLength;
                    extSST.Offsets.Add(stringOffset);
                }
                counter++;
                if (counter == extSST.NumStrings)
                {
                    counter = 0;
                }
                totalLength += stringLength;
                relativeLength += stringLength;
            }

            return extSST;
        }
    }
}
