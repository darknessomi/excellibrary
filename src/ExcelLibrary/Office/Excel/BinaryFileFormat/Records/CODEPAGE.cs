using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class CODEPAGE : Record
	{
		public CODEPAGE(Record record) : base(record) { }

		public CODEPAGE()
		{
			this.Type = RecordType.CODEPAGE;
		}

		/// <summary>
		/// text encoding used to write byte strings
		/// </summary>
		public UInt16 CodePageIdentifier;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.CodePageIdentifier = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(CodePageIdentifier);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
