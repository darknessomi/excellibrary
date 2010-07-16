using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class BOUNDSHEET : Record
	{
		public BOUNDSHEET(Record record) : base(record) { }

		public BOUNDSHEET()
		{
			this.Type = RecordType.BOUNDSHEET;
		}

		/// <summary>
		/// Absolute stream position of the BOF record of the sheet represented by this record.
		/// </summary>
		public UInt32 StreamPosition;

		/// <summary>
		/// 00H = Visible, 01H = Hidden, 02H = Strong hidden
		/// </summary>
		public Byte Visibility;

		/// <summary>
		/// 00H = Worksheet, 02H = Chart, 06H = Visual Basic module
		/// </summary>
		public Byte SheetType;

		/// <summary>
		/// BIFF8: Unicode string, 8-bit string length
		/// </summary>
		public String SheetName;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.StreamPosition = reader.ReadUInt32();
			this.Visibility = reader.ReadByte();
			this.SheetType = reader.ReadByte();
			this.SheetName = this.ReadString(reader, 8);
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(StreamPosition);
			writer.Write(Visibility);
			writer.Write(SheetType);
			Record.WriteString(writer, SheetName, 8);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
