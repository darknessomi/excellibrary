using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class BOOKBOOL : Record
	{
		public BOOKBOOL(Record record) : base(record) { }

		public BOOKBOOL()
		{
			this.Type = RecordType.BOOKBOOL;
		}

		/// <summary>
		/// 0 = Save external linked values; 1 = Do not save external linked values
		/// </summary>
		public UInt16 NotSaveExternalLinkedValues;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.NotSaveExternalLinkedValues = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(NotSaveExternalLinkedValues);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
