using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;
using ExcelLibrary.BinaryDrawingFormat;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class MSOCONTAINER : Record
	{
        public List<EscherRecord> EscherRecords = new List<EscherRecord>();

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(AllData);
            EscherRecords.Clear();
            while (stream.Position < stream.Length)
            {
                EscherRecord record = EscherRecord.Read(stream);
                record.Decode();
                EscherRecords.Add(record);
            }
        }

        public override void Encode()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            foreach (EscherRecord record in EscherRecords)
            {
                record.Encode();
                record.Write(writer);
            }
            this.Data = stream.ToArray();
            this.Size = (UInt16)Data.Length;

            this.ContinuedRecords.Clear();
            if (Size > 0 && Data.Length > MaxContentLength)
            {
                int index = MaxContentLength;
                while (index < Data.Length)
                {
                    int size = Math.Min(MaxContentLength, Data.Length - index);
                    Record continuedRecord = new Record();
                    continuedRecord.Type = this.Type;
                    continuedRecord.Size = (ushort)size;
                    continuedRecord.Data = Algorithm.ArraySection(Data, index, size);
                    this.ContinuedRecords.Add(continuedRecord);
                    index += size;
                }
                this.Size = MaxContentLength;
                this.Data = Algorithm.ArraySection(Data, 0, MaxContentLength);
            }
        }

        public TRecord FindChild<TRecord>() where TRecord : EscherRecord
        {
            foreach (EscherRecord record in EscherRecords)
            {
                if (record is TRecord)
                {
                    return record as TRecord;
                }
            }
            return null;
        }
	}
}
