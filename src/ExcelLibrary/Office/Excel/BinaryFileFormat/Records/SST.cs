using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// List of nm Unicode strings, 16-bit string length
	/// </summary>
	public partial class SST : Record
	{
		public SST(Record record) : base(record) { }

		public SST()
		{
			this.Type = RecordType.SST;
			this.StringList = new UniqueList<String>();
		}

		/// <summary>
		/// Total number of strings in the workbook
		/// </summary>
		public Int32 TotalOccurance;

		/// <summary>
		/// Number of following strings (nm)
		/// </summary>
		public Int32 NumStrings;

		/// <summary>
		/// List of nm Unicode strings, 16-bit string length
		/// </summary>
		public UniqueList<String> StringList;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.TotalOccurance = reader.ReadInt32();
			this.NumStrings = reader.ReadInt32();
			reader.ReadString();
		}

		public void encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(TotalOccurance);
			writer.Write(NumStrings);
			foreach(String stringVar in StringList)
			{
				writer.Write(stringVar);
			}
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
