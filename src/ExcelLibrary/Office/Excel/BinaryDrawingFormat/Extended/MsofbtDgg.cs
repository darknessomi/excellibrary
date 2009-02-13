using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
    public partial class MsofbtDgg : EscherRecord
    {
        /// <summary>
        /// (DrawingGroupId, numShapeIdsUsed) pairs
        /// </summary>
        public Dictionary<int, int> GroupIdClusters = new Dictionary<int, int>();

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

        public override void Encode()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(MaxShapeID);
            writer.Write(GetNumIDClusters());
            writer.Write(NumSavedShapes);
            writer.Write(NumSavedDrawings);
            List<int> groupIds = new List<int>(GroupIdClusters.Keys);
            groupIds.Sort(); //In Excel the clusters are sorted but in PPT they are not
            foreach (int groudID in groupIds)
            {
                writer.Write(groudID);
                writer.Write(GroupIdClusters[groudID]);
            }
            this.Data = stream.ToArray();
            this.Size = (UInt32)Data.Length;
            base.Encode();
        }

        public int GetNumIDClusters()
        {
            // for some reason the number of clusters is actually the real number + 1
            return GroupIdClusters.Count + 1;
        }

    }
}
