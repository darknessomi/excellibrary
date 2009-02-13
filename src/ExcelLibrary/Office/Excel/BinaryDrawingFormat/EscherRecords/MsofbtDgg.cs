using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtDgg : EscherRecord
	{
		public MsofbtDgg(EscherRecord record) : base(record) { }

		public MsofbtDgg()
		{
			this.Type = EscherRecordType.MsofbtDgg;
			this.IDClusters = new List<Int64>();
		}

		public Int32 MaxShapeID;

		public Int32 NumIDClusters;

		public Int32 NumSavedShapes;

		public Int32 NumSavedDrawings;

		public List<Int64> IDClusters;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.MaxShapeID = reader.ReadInt32();
			this.NumIDClusters = reader.ReadInt32();
			this.NumSavedShapes = reader.ReadInt32();
			this.NumSavedDrawings = reader.ReadInt32();
			reader.ReadInt64();
		}

		public void encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(MaxShapeID);
			writer.Write(NumIDClusters);
			writer.Write(NumSavedShapes);
			writer.Write(NumSavedDrawings);
			foreach(Int64 int64Var in IDClusters)
			{
				writer.Write(int64Var);
			}
			this.Data = stream.ToArray();
			this.Size = (UInt32)Data.Length;
			base.Encode();
		}

	}
}
