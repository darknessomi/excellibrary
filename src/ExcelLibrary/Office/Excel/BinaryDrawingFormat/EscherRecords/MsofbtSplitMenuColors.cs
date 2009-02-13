using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtSplitMenuColors : EscherRecord
	{
		public MsofbtSplitMenuColors(EscherRecord record) : base(record) { }

		public MsofbtSplitMenuColors()
		{
			this.Type = EscherRecordType.MsofbtSplitMenuColors;
		}

		public Int32 Color1;

		public Int32 Color2;

		public Int32 Color3;

		public Int32 Color4;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Color1 = reader.ReadInt32();
			this.Color2 = reader.ReadInt32();
			this.Color3 = reader.ReadInt32();
			this.Color4 = reader.ReadInt32();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(Color1);
			writer.Write(Color2);
			writer.Write(Color3);
			writer.Write(Color4);
			this.Data = stream.ToArray();
			this.Size = (UInt32)Data.Length;
			base.Encode();
		}

	}
}
