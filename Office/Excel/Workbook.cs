using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using QiHe.Office.CompoundDocumentFormat;

namespace QiHe.Office.Excel
{
    public class Workbook
    {
        public List<Worksheet> Worksheets = new List<Worksheet>();

        public MSODRAWINGGROUP DrawingGroup;

        public List<Record> Records;

        /// <summary>
        /// Open workbook from a file path.
        /// </summary>
        /// <param name="file"></param>
        public static Workbook Open(string file)
        {
            CompoundDocument doc = CompoundDocument.Read(file);
            if (doc == null) throw new Exception("Invalid Excel file");
            byte[] bookdata = doc.GetStreamData("Workbook");
            return WorkbookDecoder.Decode(new MemoryStream(bookdata));
        }

        public void Save(string file)
        {
            CompoundDocument doc = CompoundDocument.Create(file);
            MemoryStream stream = new MemoryStream();
            WorkbookEncoder.Encode(this, stream);
            doc.WriteStreamData(new string[] { "Workbook" }, stream.ToArray());
            doc.Save();
            doc.Close();
        }

        public List<byte[]> ExtractImages()
        {
            List<byte[]> Images = new List<byte[]>();
            if (DrawingGroup != null)
            {
                MsofbtDggContainer dggContainer = DrawingGroup.EscherRecords[0] as MsofbtDggContainer;
                foreach (MsofbtBSE blipStoreEntry in dggContainer.BstoreContainer.EscherRecords)
                {
                    if (blipStoreEntry.BlipRecord == null) continue;
                    Images.Add(blipStoreEntry.ImageData);
                }
            }
            return Images;
        }

        public byte[] ExtractImage(int index, out ushort type)
        {
            if (DrawingGroup != null)
            {
                MsofbtDggContainer dggContainer = DrawingGroup.EscherRecords[0] as MsofbtDggContainer;
                MsofbtBSE blipStoreEntry = dggContainer.BstoreContainer.EscherRecords[index] as MsofbtBSE;
                if (blipStoreEntry.BlipRecord != null)
                {
                    type = blipStoreEntry.BlipRecord.Type;
                    return blipStoreEntry.ImageData;
                }
            }
            type = 0;
            return null;
        }

        internal void RemoveRecord(int index)
        {
            Record rec = Records[index];
            foreach (Record record in Records)
            {
                if (record.Type == RecordType.BOUNDSHEET)
                {
                    //Points to worksheet BOF record
                    ((BOUNDSHEET)record).StreamPosition -= (uint)rec.FullSize;
                    record.Encode();
                }
                if (record.Type == RecordType.EXTSST)
                {
                    foreach (StringOffset offset in ((EXTSST)record).Offsets)
                    {
                        offset.AbsolutePosition -= (uint)rec.FullSize;
                    }
                    record.Encode();
                }
                if (rec.Type == RecordType.XF && record is CellValue)
                {
                    (record as CellValue).XFIndex = 0;
                    record.Encode();
                }
            }
            Records.RemoveAt(index);
        }
    }
}
