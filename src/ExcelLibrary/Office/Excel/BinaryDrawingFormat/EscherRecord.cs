using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
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
        /// Instance
        /// </summary>
        public UInt16 Instance
        {
            get { return (UInt16)(Prop >> 4); }
            set { Prop = (UInt16)(Version | (value << 4)); }
        }

        /// <summary>
        /// Version
        /// </summary>
        public byte Version
        {
            get { return (byte)(Prop & 0xF); }
            set { Prop = (UInt16)(Prop | (value & 0xF)); }
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

        //ushort inst = 0;
        public void Write(BinaryWriter writer)
        {
            if (this is MsofbtContainer)
            {
                Version = 0xF;
            }
            else
            {
                //Instance = inst++;
            }
            writer.Write(this.Prop);
            writer.Write(this.Type);
            writer.Write(this.Size);
            if (this.Size > 0)
            {
                writer.Write(this.Data);
            }
        }
    }
}
