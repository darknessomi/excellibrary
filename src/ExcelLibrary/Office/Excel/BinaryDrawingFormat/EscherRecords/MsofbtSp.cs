using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtSp : EscherRecord
	{
		public MsofbtSp(EscherRecord record) : base(record) { }

		public MsofbtSp()
		{
			this.Type = EscherRecordType.MsofbtSp;
		}

		public Int32 ShapeId;

		public Int32 Flags;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.ShapeId = reader.ReadInt32();
			this.Flags = reader.ReadInt32();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(ShapeId);
			writer.Write(Flags);
			this.Data = stream.ToArray();
			this.Size = (UInt32)Data.Length;
			base.Encode();
		}

	}
}
