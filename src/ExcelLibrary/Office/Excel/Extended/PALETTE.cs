using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class PALETTE : Record
	{
		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			NumColors = reader.ReadUInt16();
            RGBColours = new List<int>(NumColors);
            for (int i = 0; i < NumColors; i++)
            {
                RGBColours.Add(reader.ReadInt32());
            }
		}

    }
}
