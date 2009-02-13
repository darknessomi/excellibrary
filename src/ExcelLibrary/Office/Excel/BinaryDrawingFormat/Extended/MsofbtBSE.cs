using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;

namespace ExcelLibrary.BinaryDrawingFormat
{
    /// <summary>
    /// File BLIP Store Entry 
    /// </summary>
    public partial class MsofbtBSE : EscherRecord
    {
        public string BlipName;
        public MsofbtBlip BlipRecord;
        public byte[] ImageData;
        public byte[] ExtraData;

        internal void SetBlipType(ushort imageFormat)
        {
            byte bliptype = BlipType.FromImageFormat(imageFormat);
            Instance = bliptype;
            BlipTypeWin32 = bliptype;
            BlipTypeMacOS = bliptype;
            BlipRecord.Instance = BlipSignature.FromBlipType(bliptype);
        }

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
            BinaryReader reader = new BinaryReader(stream);
            BlipTypeWin32 = reader.ReadByte();
            BlipTypeMacOS = reader.ReadByte();
            UID = new Guid(reader.ReadBytes(16));
            Tag = reader.ReadUInt16();
            BlipSize = reader.ReadUInt32();
            Ref = reader.ReadInt32();
            Offset = reader.ReadInt32();
            Usage = reader.ReadByte();
            NameLength = reader.ReadByte();
            Unused2 = reader.ReadByte();
            Unused3 = reader.ReadByte();

            if (NameLength > 0)
            {
                BlipName = Encoding.Unicode.GetString(reader.ReadBytes(NameLength));
            }

            if (stream.Position < stream.Length)
            {
                BlipRecord = EscherRecord.Read(stream) as MsofbtBlip;
                if (BlipRecord != null)
                {
                    BlipRecord.Decode();
                    this.ImageData = BlipRecord.ImageData;
                }
                else
                {
                    throw new Exception("Image Type Not supported.");
                }
            }

            if (stream.Position < stream.Length)
            {
                ExtraData = StreamHelper.ReadToEnd(stream);
            }
        }

        public override void Encode()
        {
            this.BlipRecord.Encode();
            this.BlipSize = BlipRecord.Size + 8;
            if (BlipName != null) NameLength = (byte)BlipName.Length;

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(BlipTypeWin32);
            writer.Write(BlipTypeMacOS);
            writer.Write(UID.ToByteArray());
            writer.Write(Tag);
            writer.Write(BlipSize);
            writer.Write(Ref);
            writer.Write(Offset);
            writer.Write(Usage);
            writer.Write(NameLength);
            writer.Write(Unused2);
            writer.Write(Unused3);

            if (NameLength > 0)
            {
                writer.Write(Encoding.Unicode.GetBytes(BlipName));
            }

            this.BlipRecord.Write(writer);

            this.Data = stream.ToArray();
            this.Size = (UInt16)Data.Length;
            base.Encode();
        }
    }
}
