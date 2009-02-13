using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class MULRK : Record
	{
        public List<UInt32> RKList;
        public List<UInt16> XFList;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			RowIndex = reader.ReadUInt16();
			FirstColIndex = reader.ReadUInt16();

            int count = (Size - 6) / 6;
            RKList = new List<uint>(count);
            XFList = new List<ushort>(count);
            for (int i = 0; i < count; i++)
            {
                UInt16 XFIndex = reader.ReadUInt16();
                UInt32 RKValue = reader.ReadUInt32();
                XFList.Add(XFIndex);
                RKList.Add(RKValue);
            }

			LastColIndex = reader.ReadInt16();
		}

	}
}
