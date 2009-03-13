using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
    /// <summary>
    /// This record occurs in conjunction with the SST record.
    /// Used to optimise string search operations.
    /// </summary>
    public partial class EXTSST : Record
    {
        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(AllData);
            BinaryReader reader = new BinaryReader(stream);
            this.NumStrings = reader.ReadUInt16();
            this.Offsets = new List<StringOffset>();
            while (stream.Position < stream.Length)
            {
               this.Offsets.Add(ReadStringOffset(reader));
            }
        }

        public override void Encode()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(NumStrings);
            foreach (StringOffset stringoffsetVar in Offsets)
            {
                WriteStringOffset(writer, stringoffsetVar);
            }
            this.Data = stream.ToArray();
            this.Size = (UInt16)Data.Length;
            base.Encode();
        }

        static StringOffset ReadStringOffset(BinaryReader reader)
        {
            StringOffset stringOffset = new StringOffset();
            stringOffset.AbsolutePosition = reader.ReadUInt32();
            stringOffset.RelativePosition = reader.ReadUInt16();
            stringOffset.NotUsed = reader.ReadUInt16();
            return stringOffset;
        }

        static void WriteStringOffset(BinaryWriter writer, StringOffset stringoffset)
        {
            writer.Write(stringoffset.AbsolutePosition);
            writer.Write(stringoffset.RelativePosition);
            writer.Write(stringoffset.NotUsed);
        }

    }
}
