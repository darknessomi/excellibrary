using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class OBJ : Record
	{
        public List<SubRecord> SubRecords = new List<SubRecord>();

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
            SubRecords.Clear();
            while (stream.Position < Size)
            {
                SubRecord subRecord = SubRecord.Read(stream);
                subRecord.Decode();
                SubRecords.Add(subRecord);
            }
        }

        public override void Encode()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            foreach (SubRecord subRecord in SubRecords)
            {
                subRecord.Encode();
                subRecord.Write(writer);
            }
            this.Data = stream.ToArray();
            this.Size = (UInt16)Data.Length;
        }

	}
}
