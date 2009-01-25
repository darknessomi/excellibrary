using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
    public partial class MsofbtContainer : EscherRecord
    {
        public List<EscherRecord> EscherRecords;

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
            EscherRecords = new List<EscherRecord>();
            while (stream.Position < stream.Length)
            {
                EscherRecord record = EscherRecord.Read(stream);
                record.Decode();
                EscherRecords.Add(record);
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
