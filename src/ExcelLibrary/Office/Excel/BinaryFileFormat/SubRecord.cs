using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
    public partial class SubRecord : Record
    {
        public SubRecord() : base() { }

        public SubRecord(SubRecord record) : base(record) { }

        public new static SubRecord ReadBase(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            SubRecord record = new SubRecord();
            record.Type = reader.ReadUInt16();
            record.Size = reader.ReadUInt16();
            record.Data = reader.ReadBytes(record.Size);
            return record;
        }
    }
}
