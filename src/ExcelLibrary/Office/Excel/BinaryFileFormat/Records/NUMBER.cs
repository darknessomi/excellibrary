using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class NUMBER : CellValue
	{
		public NUMBER(Record record) : base(record) { }

		public NUMBER()
		{
			this.Type = RecordType.NUMBER;
		}

		/// <summary>
		/// IEEE 754 floating-point value (64-bit double precision)
		/// </summary>
		public Double Value;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.ColIndex = reader.ReadUInt16();
			this.XFIndex = reader.ReadUInt16();
			this.Value = reader.ReadDouble();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(RowIndex);
			writer.Write(ColIndex);
			writer.Write(XFIndex);
			writer.Write(Value);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
