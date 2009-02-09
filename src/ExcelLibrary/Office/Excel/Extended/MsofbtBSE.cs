using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ExcelLibrary.CodeLib;

namespace ExcelLibrary.Office.Excel
{
    /// <summary>
    /// File BLIP Store Entry 
    /// </summary>
    public partial class MsofbtBSE : EscherRecord
    {
        public MsofbtBlip BlipRecord;
        public byte[] ImageData;
        public byte[] RemainedData;

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
            BinaryReader reader = new BinaryReader(stream);
            BlipTypeWin32 = reader.ReadByte();
            BlipTypeMacOS = reader.ReadByte();
            UID = new Guid(reader.ReadBytes(16));
            Tag = reader.ReadUInt16();
            Size = reader.ReadInt32();
            Ref = reader.ReadInt32();
            Offset = reader.ReadInt32();
            Usage = reader.ReadByte();
            NameLength = reader.ReadByte();
            Unused2 = reader.ReadByte();
            Unused3 = reader.ReadByte();

            if (stream.Position < stream.Length)
            {
                BlipRecord = EscherRecord.Read(stream) as MsofbtBlip;
                if (BlipRecord != null)
                {
                    int HeaderSize = 17;
                    ImageData = new byte[BlipRecord.Data.Length - HeaderSize];
                    Array.Copy(BlipRecord.Data, HeaderSize, ImageData, 0, ImageData.Length);
                }
                else
                {
                    throw new Exception("Image Type Not supported.");
                }
            }

            if (stream.Position < stream.Length)
            {
                RemainedData = StreamHelper.ReadToEnd(stream);
            }
        }
    }
}
