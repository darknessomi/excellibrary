using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
    public partial class EscherRecord
    {
        public UInt16 Prop;
        public UInt16 Type;
        public UInt32 Size;
        public byte[] Data;

        public EscherRecord() { }

        public EscherRecord(EscherRecord record)
        {
            Prop = record.Prop;
            Type = record.Type;
            Size = record.Size;
            Data = record.Data;
        }

        /// <summary>
        /// Instance ID
        /// </summary>
        public UInt16 ID
        {
            get { return (UInt16)(Prop >> 4); }
        }


        public virtual void Decode()
        {
        }

        public virtual void Encode()
        {
        }

        public static EscherRecord ReadBase(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            EscherRecord record = new EscherRecord();
            record.Prop = reader.ReadUInt16();
            record.Type = reader.ReadUInt16();
            record.Size = reader.ReadUInt32();
            record.Data = reader.ReadBytes((int)record.Size);
            return record;
        }
    }
}
