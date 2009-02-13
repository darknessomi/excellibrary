using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtClientAnchor : EscherRecord
	{
		public MsofbtClientAnchor(EscherRecord record) : base(record) { }

		public MsofbtClientAnchor()
		{
			this.Type = EscherRecordType.MsofbtClientAnchor;
		}

		public UInt16 Flag;

		public UInt16 Col1;

		public UInt16 DX1;

		public UInt16 Row1;

		public UInt16 DY1;

		public UInt16 Col2;

		public UInt16 DX2;

		public UInt16 Row2;

		public UInt16 DY2;

		public Byte[] ExtraData;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Flag = reader.ReadUInt16();
			this.Col1 = reader.ReadUInt16();
			this.DX1 = reader.ReadUInt16();
			this.Row1 = reader.ReadUInt16();
			this.DY1 = reader.ReadUInt16();
			this.Col2 = reader.ReadUInt16();
			this.DX2 = reader.ReadUInt16();
			this.Row2 = reader.ReadUInt16();
			this.DY2 = reader.ReadUInt16();
			this.ExtraData = reader.ReadBytes((int)(stream.Length - stream.Position));
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(Flag);
			writer.Write(Col1);
			writer.Write(DX1);
			writer.Write(Row1);
			writer.Write(DY1);
			writer.Write(Col2);
			writer.Write(DX2);
			writer.Write(Row2);
			writer.Write(DY2);
			writer.Write(ExtraData);
			this.Data = stream.ToArray();
			this.Size = (UInt32)Data.Length;
			base.Encode();
		}

	}
}
