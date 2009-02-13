using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtDg : EscherRecord
	{
		public MsofbtDg(EscherRecord record) : base(record) { }

		public MsofbtDg()
		{
			this.Type = EscherRecordType.MsofbtDg;
		}

		public Int32 NumShapes;

		public Int32 LastShapeID;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.NumShapes = reader.ReadInt32();
			this.LastShapeID = reader.ReadInt32();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(NumShapes);
			writer.Write(LastShapeID);
			this.Data = stream.ToArray();
			this.Size = (UInt32)Data.Length;
			base.Encode();
		}

	}
}
