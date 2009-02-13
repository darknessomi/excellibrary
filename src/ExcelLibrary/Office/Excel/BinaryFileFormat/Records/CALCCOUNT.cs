using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class CALCCOUNT : Record
	{
		public CALCCOUNT(Record record) : base(record) { }

		public CALCCOUNT()
		{
			this.Type = RecordType.CALCCOUNT;
		}

		/// <summary>
		/// Maximum number of iterations allowed in circular references
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
