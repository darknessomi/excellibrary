using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// contains relative offsets to calculate the stream position of the first cell
	/// record for each row in each row block.
	/// </summary>
	public partial class DBCELL : Record
	{
		public DBCELL(Record record) : base(record) { }

		public DBCELL()
		{
			this.Type = RecordType.DBCELL;
			this.FirstCellOffsets = new List<UInt16>();
		}

		/// <summary>
		/// Relative offset to first ROW record in the Row Block
		/// </summary>
		public UInt32 FirstRowOffset;

		/// <summary>
		/// relative offsets to calculate stream position of the first cell record for the respective row
		/// </summary>
		public List<UInt16> FirstCellOffsets;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.FirstRowOffset = reader.ReadUInt32();
			int count = (this.Size - 4) / 2;
			this.FirstCellOffsets = new List<UInt16>(count);
			for (int i = 0; i < count; i++)
			{
				FirstCellOffsets.Add(reader.ReadUInt16());
			}
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(FirstRowOffset);
			foreach(UInt16 uint16Var in FirstCellOffsets)
			{
				writer.Write(uint16Var);
			}
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
