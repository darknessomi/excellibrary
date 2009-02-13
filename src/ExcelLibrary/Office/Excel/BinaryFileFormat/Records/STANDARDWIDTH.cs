using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class STANDARDWIDTH : Record
	{
		public STANDARDWIDTH(Record record) : base(record) { }

		public STANDARDWIDTH()
		{
			this.Type = RecordType.STANDARDWIDTH;
		}

		/// <summary>
		/// Default width of the columns in 1/256 of the width of the zero character, using default font (first FONT record in the file)
		/// </summary>
		public UInt16 DefaultColumnWidth;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.DefaultColumnWidth = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(DefaultColumnWidth);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
