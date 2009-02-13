using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class RSTRING : Record
	{
		public RSTRING(Record record) : base(record) { }

		public RSTRING()
		{
			this.Type = RecordType.RSTRING;
		}

		/// <summary>
		/// List of rt formatting runs
		/// </summary>
		public UInt32 FormattingRuns;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.FormattingRuns = reader.ReadUInt32();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(FormattingRuns);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
