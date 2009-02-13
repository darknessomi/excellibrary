using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// This record stores the result of a string formula. It occurs directly after a string formula.
	/// </summary>
	public partial class STRING : Record
	{
		public STRING(Record record) : base(record) { }

		public STRING()
		{
			this.Type = RecordType.STRING;
		}

		/// <summary>
		/// Non-empty Unicode string, 16-bit string length
		/// </summary>
		public String Value;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Value = this.ReadString(reader, 16);
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			Record.WriteString(writer, Value,16);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
