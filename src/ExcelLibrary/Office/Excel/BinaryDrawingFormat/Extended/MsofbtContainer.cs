using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
    public partial class MsofbtContainer : EscherRecord
    {
        public List<EscherRecord> EscherRecords = new List<EscherRecord>();

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
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
            this.Size = (UInt32)Data.Length;
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

        public List<TRecord> FindChildren<TRecord>() where TRecord : EscherRecord
        {
            List<TRecord> children = new List<TRecord>();
            foreach (EscherRecord record in EscherRecords)
            {
                if (record is TRecord)
                {
                    children.Add(record as TRecord);
                }
            }
            return children;
        }

    }
}
