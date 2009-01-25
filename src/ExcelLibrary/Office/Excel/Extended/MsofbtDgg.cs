using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtDgg : EscherRecord
	{
        Dictionary<int, int> GroupIdClusters = new Dictionary<int, int>();
		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			MaxShapeID = reader.ReadInt32();
			NumIDClusters = reader.ReadInt32();
			NumSavedShapes = reader.ReadInt32();
			NumSavedDrawings = reader.ReadInt32();
            IDClusters = new List<long>();
            while (stream.Position < stream.Length)
            {
                //IDClusters.Add(reader.ReadInt64());
                int drawingGroupId = reader.ReadInt32();
                int numShapeIdsUsed = reader.ReadInt32();
                GroupIdClusters.Add(drawingGroupId, numShapeIdsUsed);
            }
		}

	}
}
