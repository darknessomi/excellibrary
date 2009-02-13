using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class ClipboardFormat : SubRecord
	{
		public ClipboardFormat(SubRecord record) : base(record) { }

		public ClipboardFormat()
		{
			this.Type = SubRecordType.ClipboardFormat;
		}

		public UInt16 Reserved;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Reserved = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(Reserved);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
