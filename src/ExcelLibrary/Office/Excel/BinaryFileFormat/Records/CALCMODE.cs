using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class CALCMODE : Record
	{
		public CALCMODE(Record record) : base(record) { }

		public CALCMODE()
		{
			this.Type = RecordType.CALCMODE;
		}

		/// <summary>
		/// whether to calculate formulas manually,automatically or automatically except for multiple table operations.
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
