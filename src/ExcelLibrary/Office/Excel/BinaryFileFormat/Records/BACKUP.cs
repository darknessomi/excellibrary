using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class BACKUP : Record
	{
		public BACKUP(Record record) : base(record) { }

		public BACKUP()
		{
			this.Type = RecordType.BACKUP;
		}

		/// <summary>
		/// whether Excel makes a backup of the file while saving
		/// </summary>
		public UInt16 CreateBackupOnSaving;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.CreateBackupOnSaving = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(CreateBackupOnSaving);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
