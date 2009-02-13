using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class XF : Record
	{
		public XF(Record record) : base(record) { }

		public XF()
		{
			this.Type = RecordType.XF;
		}

		public UInt16 FontIndex;

		public UInt16 FormatIndex;

		public UInt16 CellProtection;

		public Byte Alignment;

		public Byte Rotation;

		public Byte Indent;

		public Byte Attributes;

		public UInt32 LineStyle;

		public UInt32 LineColor;

		public UInt16 Background;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.FontIndex = reader.ReadUInt16();
			this.FormatIndex = reader.ReadUInt16();
			this.CellProtection = reader.ReadUInt16();
			this.Alignment = reader.ReadByte();
			this.Rotation = reader.ReadByte();
			this.Indent = reader.ReadByte();
			this.Attributes = reader.ReadByte();
			this.LineStyle = reader.ReadUInt32();
			this.LineColor = reader.ReadUInt32();
			this.Background = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(FontIndex);
			writer.Write(FormatIndex);
			writer.Write(CellProtection);
			writer.Write(Alignment);
			writer.Write(Rotation);
			writer.Write(Indent);
			writer.Write(Attributes);
			writer.Write(LineStyle);
			writer.Write(LineColor);
			writer.Write(Background);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
