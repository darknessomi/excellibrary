using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class MULRK : Record
	{
		public MULRK(Record record) : base(record) { }

		public MULRK()
		{
			this.Type = RecordType.MULRK;
			this.XFRKList = new List<UInt32>();
		}

		/// <summary>
		/// Index to row
		/// </summary>
		public UInt16 RowIndex;

		/// <summary>
		/// Index to first column (fc)
		/// </summary>
		public UInt16 FirstColIndex;

		/// <summary>
		/// List of nc=lc-fc+1 XF/RK structures.
		/// </summary>
		public List<UInt32> XFRKList;

		/// <summary>
		/// Index to last column (lc)
		/// </summary>
		public Int16 LastColIndex;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.FirstColIndex = reader.ReadUInt16();
			int count = (Size - 6) / 6;
			this.XFRKList = new List<UInt32>(count);
			for (int i = 0; i < count; i++)
			{
				XFRKList.Add(reader.ReadUInt32());
			}
			this.LastColIndex = reader.ReadInt16();
		}

		public void encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(RowIndex);
			writer.Write(FirstColIndex);
			foreach(UInt32 uint32Var in XFRKList)
			{
				writer.Write(uint32Var);
			}
			writer.Write(LastColIndex);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
