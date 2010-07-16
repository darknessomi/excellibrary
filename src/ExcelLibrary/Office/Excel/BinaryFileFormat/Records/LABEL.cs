using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class LABEL : CellValue
	{
		public LABEL(Record record) : base(record) { }

		public LABEL()
		{
			this.Type = RecordType.LABEL;
		}

		/// <summary>
		/// Index into SST record
		/// </summary>
		public String Value;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.ColIndex = reader.ReadUInt16();
			this.XFIndex = reader.ReadUInt16();
			this.Value = this.ReadString(reader, 16);
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(RowIndex);
			writer.Write(ColIndex);
			writer.Write(XFIndex);
			Record.WriteString(writer, Value, 16);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
