using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class PALETTE : Record
	{
		public PALETTE(Record record) : base(record) { }

		public PALETTE()
		{
			this.Type = RecordType.PALETTE;
			this.RGBColours = new List<Int32>();
		}

		/// <summary>
		/// Number of following colours (nm).
		/// </summary>
		public UInt16 NumColors;

		/// <summary>
		/// List of nm RGB colours
		/// </summary>
		public List<Int32> RGBColours;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.NumColors = reader.ReadUInt16();
			reader.ReadInt32();
		}

		public void encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(NumColors);
			foreach(Int32 int32Var in RGBColours)
			{
				writer.Write(int32Var);
			}
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
