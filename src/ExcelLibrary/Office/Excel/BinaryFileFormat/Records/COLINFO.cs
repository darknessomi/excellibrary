using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// This record specifies the width and default cell formatting for a given range of columns.
	/// </summary>
	public partial class COLINFO : Record
	{
		public COLINFO(Record record) : base(record) { }

		public COLINFO()
		{
			this.Type = RecordType.COLINFO;
		}

		/// <summary>
		/// Index to first column in the range
		/// </summary>
		public UInt16 FirstColIndex;

		/// <summary>
		/// Index to last column in the range
		/// </summary>
		public UInt16 LastColIndex;

		/// <summary>
		/// Width of the columns in 1/256 of the width of the zero character, using default font (first
		/// FONT record in the file)
		/// </summary>
		public UInt16 Width;

		/// <summary>
		/// Index to XF record for default column formatting
		/// </summary>
		public UInt16 XFIndex;

		/// <summary>
		/// Option flags
		/// </summary>
		public UInt16 OptionFlags;

		/// <summary>
		/// Not used
		/// </summary>
		public UInt16 NotUsed;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.FirstColIndex = reader.ReadUInt16();
			this.LastColIndex = reader.ReadUInt16();
			this.Width = reader.ReadUInt16();
			this.XFIndex = reader.ReadUInt16();
			this.OptionFlags = reader.ReadUInt16();
			this.NotUsed = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(FirstColIndex);
			writer.Write(LastColIndex);
			writer.Write(Width);
			writer.Write(XFIndex);
			writer.Write(OptionFlags);
			writer.Write(NotUsed);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
