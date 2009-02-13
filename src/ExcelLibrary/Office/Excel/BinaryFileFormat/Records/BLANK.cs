using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class BLANK : Record
	{
		public BLANK(Record record) : base(record) { }

		public BLANK()
		{
			this.Type = RecordType.BLANK;
		}

		public UInt16 RowIndex;

		public UInt16 ColIndex;

		public UInt16 XFIndex;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.ColIndex = reader.ReadUInt16();
			this.XFIndex = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(RowIndex);
			writer.Write(ColIndex);
			writer.Write(XFIndex);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
