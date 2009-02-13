using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtSpgr : EscherRecord
	{
		public MsofbtSpgr(EscherRecord record) : base(record) { }

		public MsofbtSpgr()
		{
			this.Type = EscherRecordType.MsofbtSpgr;
		}

		public Int32 Left;

		public Int32 Top;

		public Int32 Right;

		public Int32 Bottom;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Left = reader.ReadInt32();
			this.Top = reader.ReadInt32();
			this.Right = reader.ReadInt32();
			this.Bottom = reader.ReadInt32();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(Left);
			writer.Write(Top);
			writer.Write(Right);
			writer.Write(Bottom);
			this.Data = stream.ToArray();
			this.Size = (UInt32)Data.Length;
			base.Encode();
		}

	}
}
