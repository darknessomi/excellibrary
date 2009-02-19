using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;

namespace ExcelLibrary.BinaryFileFormat
{
    public partial class SST : Record
    {
        public RichTextFormat[] RichTextFormatting;

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
            BinaryReader reader = new BinaryReader(stream);
            TotalOccurance = reader.ReadInt32();
            NumStrings = reader.ReadInt32();
            StringList = new UniqueList<string>(NumStrings);
            RichTextFormatting = new RichTextFormat[NumStrings];
            StringDecoder stringDecoder = new StringDecoder(this, reader);
            for (int i = 0; i < NumStrings; i++)
            {
                StringList.Add(stringDecoder.ReadString(16, out RichTextFormatting[i]));
            }
        }

        public override void Encode()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            NumStrings = StringList.Count;
            writer.Write(TotalOccurance);
            writer.Write(NumStrings);
            this.ContinuedRecords.Clear();
            Record currentRecord = this;
            foreach (String stringVar in StringList)
            {
                int stringlength = Record.GetStringDataLength(stringVar);
                if (stream.Length + stringlength > Record.MaxContentLength)
                {
                    currentRecord.Data = stream.ToArray();
                    currentRecord.Size = (UInt16)currentRecord.Data.Length;

                    stream = new MemoryStream();
                    writer = new BinaryWriter(stream);

                    CONTINUE continuedRecord = new CONTINUE();
                    this.ContinuedRecords.Add(continuedRecord);
                    currentRecord = continuedRecord;
                }
                Record.WriteString(writer, stringVar, 16);
            }
            currentRecord.Data = stream.ToArray();
            currentRecord.Size = (UInt16)currentRecord.Data.Length;
        }
    }
}
