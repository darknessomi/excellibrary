using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// This record occurs in conjunction with the SST record.
	/// Used to optimise string search operations.
	/// </summary>
	public partial class EXTSST : Record
	{
		public EXTSST(Record record) : base(record) { }

		public EXTSST()
		{
			this.Type = RecordType.EXTSST;
			this.Offsets = new List<StringOffset>();
		}

		/// <summary>
		/// Number of strings in a portion, this number is  >=8
		/// </summary>
		public UInt16 NumStrings;

		/// <summary>
		/// List of OFFSET structures for all portions.
		/// </summary>
		public List<StringOffset> Offsets;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.NumStrings = reader.ReadUInt16();
			ReadStringOffset(reader);
		}

		public void encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(NumStrings);
			foreach(StringOffset stringoffsetVar in Offsets)
			{
				WriteStringOffset(writer, stringoffsetVar);
			}
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
