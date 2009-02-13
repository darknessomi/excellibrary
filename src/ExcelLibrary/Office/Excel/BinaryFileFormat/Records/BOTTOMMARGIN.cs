using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class BOTTOMMARGIN : Record
	{
		public BOTTOMMARGIN(Record record) : base(record) { }

		public BOTTOMMARGIN()
		{
			this.Type = RecordType.BOTTOMMARGIN;
		}

		/// <summary>
		/// Bottom page margin in inches (IEEE 754 floating-point value, 64-bit double precision)
		/// </summary>
		public Double Value;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Value = reader.ReadDouble();
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
