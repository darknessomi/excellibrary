using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
    public partial class BOF : Record
    {
        public BOF(Record record) : base(record) { }

        public BOF()
        {
            this.Type = RecordType.BOF;
        }

        public UInt16 BIFFversion;

        public UInt16 StreamType;

        public UInt16 BuildID;

        public UInt16 BuildYear;

        public UInt32 FileHistoryFlags;

        /// <summary>
        /// Lowest Excel version that can read all records in this file
        /// </summary>
        public UInt32 RequiredExcelVersion;

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
            BinaryReader reader = new BinaryReader(stream);
            this.BIFFversion = reader.ReadUInt16();
            this.StreamType = reader.ReadUInt16();
            if (Data.Length >= 6) this.BuildID = reader.ReadUInt16();
            if (Data.Length >= 8) this.BuildYear = reader.ReadUInt16();
            if (Data.Length >= 12) this.FileHistoryFlags = reader.ReadUInt32();
            if (Data.Length >= 16) this.RequiredExcelVersion = reader.ReadUInt32();
        }

        public override void Encode()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(BIFFversion);
            writer.Write(StreamType);
            writer.Write(BuildID);
            writer.Write(BuildYear);
            writer.Write(FileHistoryFlags);
            writer.Write(RequiredExcelVersion);
            this.Data = stream.ToArray();
            this.Size = (UInt16)Data.Length;
            base.Encode();
        }

    }
}
