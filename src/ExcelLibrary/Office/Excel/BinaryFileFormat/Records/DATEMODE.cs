using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class DATEMODE : Record
	{
		public DATEMODE(Record record) : base(record) { }

		public DATEMODE()
		{
			this.Type = RecordType.DATEMODE;
		}

		/// <summary>
		/// 0 = Base date is 1899-Dec-31; 1 = Base date is 1904-Jan-01
		/// </summary>
		public Int16 Mode;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Mode = reader.ReadInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(Mode);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
