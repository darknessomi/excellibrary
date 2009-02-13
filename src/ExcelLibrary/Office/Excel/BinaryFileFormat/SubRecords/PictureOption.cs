using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// Picture option flags
	/// </summary>
	public partial class PictureOption : SubRecord
	{
		public PictureOption(SubRecord record) : base(record) { }

		public PictureOption()
		{
			this.Type = SubRecordType.PictureOption;
		}

		/// <summary>
		/// Picture option flags
		/// </summary>
		public UInt16 Reserved;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Reserved = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(Reserved);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
