using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class BOOLERR : CellValue
	{
		public BOOLERR(Record record) : base(record) { }

		public BOOLERR()
		{
			this.Type = RecordType.BOOLERR;
		}

		/// <summary>
		/// Boolean or error value (type depends on the following byte)
		/// </summary>
		public Byte Value;

		/// <summary>
		/// 0 = Boolean value; 1 = Error code
		/// </summary>
		public Byte ValueType;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.ColIndex = reader.ReadUInt16();
			this.XFIndex = reader.ReadUInt16();
			this.Value = reader.ReadByte();
			this.ValueType = reader.ReadByte();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(RowIndex);
			writer.Write(ColIndex);
			writer.Write(XFIndex);
			writer.Write(Value);
			writer.Write(ValueType);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
