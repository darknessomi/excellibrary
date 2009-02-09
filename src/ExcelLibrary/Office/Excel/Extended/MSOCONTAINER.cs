using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MSOCONTAINER : Record
	{
        public List<EscherRecord> EscherRecords;

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(AllData);
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
	}
}
