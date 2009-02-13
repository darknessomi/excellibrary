using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// This record contains information about a number format.
	/// </summary>
	public partial class FORMAT : Record
	{
		public FORMAT(Record record) : base(record) { }

		public FORMAT()
		{
			this.Type = RecordType.FORMAT;
		}

		/// <summary>
		/// Format index used in other records
		/// </summary>
		public UInt16 FormatIndex;

		/// <summary>
		/// Number format string
		/// </summary>
		public String FormatString;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.FormatIndex = reader.ReadUInt16();
			this.FormatString = this.ReadString(reader, 16);
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(FormatIndex);
			Record.WriteString(writer, FormatString,16);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
