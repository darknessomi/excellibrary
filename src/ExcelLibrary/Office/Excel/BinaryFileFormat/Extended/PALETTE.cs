using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class PALETTE : Record
	{
		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			NumColors = reader.ReadInt16();
            Colors = new List<int>(NumColors);
            for (int i = 0; i < NumColors; i++)
            {
                Colors.Add(reader.ReadInt32());
            }
        }

        public override void Encode()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(NumColors);
            foreach (Int32 int32Var in Colors)
            {
                writer.Write(int32Var);
            }
            this.Data = stream.ToArray();
            this.Size = (UInt16)Data.Length;
            base.Encode();
        }

    }
}
