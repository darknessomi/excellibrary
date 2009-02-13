using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// This record specifies  the default  column width for columns  that  do not  have a specific width set  using  the records
	/// COLWIDTH, COLINFO or STANDARDWIDTH.
	/// </summary>
	public partial class DEFCOLWIDTH : Record
	{
		public DEFCOLWIDTH(Record record) : base(record) { }

		public DEFCOLWIDTH()
		{
			this.Type = RecordType.DEFCOLWIDTH;
		}

		/// <summary>
		/// Column width in characters, using the width of the zero character from default font (first FONT record in the file).
		/// </summary>
		public UInt16 Value;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Value = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(Value);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
