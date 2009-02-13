using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class DIMENSIONS : Record
	{
		public DIMENSIONS(Record record) : base(record) { }

		public DIMENSIONS()
		{
			this.Type = RecordType.DIMENSIONS;
		}

		/// <summary>
		/// Index to first used row
		/// </summary>
		public Int32 FirstRow;

		/// <summary>
		/// Index to last used row, increased by 1
		/// </summary>
		public Int32 LastRow;

		/// <summary>
		/// Index to first used column
		/// </summary>
		public Int16 FirstColumn;

		/// <summary>
		/// Index to last used column, increased by 1
		/// </summary>
		public Int16 LastColumn;

		/// <summary>
		/// Not used
		/// </summary>
		public Int16 UnUsed;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.FirstRow = reader.ReadInt32();
			this.LastRow = reader.ReadInt32();
			this.FirstColumn = reader.ReadInt16();
			this.LastColumn = reader.ReadInt16();
			this.UnUsed = reader.ReadInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(FirstRow);
			writer.Write(LastRow);
			writer.Write(FirstColumn);
			writer.Write(LastColumn);
			writer.Write(UnUsed);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
