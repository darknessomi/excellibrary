using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtBSE : EscherRecord
	{
		public MsofbtBSE(EscherRecord record) : base(record) { }

		public MsofbtBSE()
		{
			this.Type = EscherRecordType.MsofbtBSE;
		}

		public Byte BlipTypeWin32;

		public Byte BlipTypeMacOS;

		public Guid UID;

		public UInt16 Tag;

		public UInt32 BlipSize;

		public Int32 Ref;

		public Int32 Offset ;

		public Byte Usage;

		public Byte NameLength;

		public Byte Unused2;

		public Byte Unused3;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.BlipTypeWin32 = reader.ReadByte();
			this.BlipTypeMacOS = reader.ReadByte();
			this.UID = new Guid(reader.ReadBytes(16));
			this.Tag = reader.ReadUInt16();
			this.BlipSize = reader.ReadUInt32();
			this.Ref = reader.ReadInt32();
			this.Offset  = reader.ReadInt32();
			this.Usage = reader.ReadByte();
			this.NameLength = reader.ReadByte();
			this.Unused2 = reader.ReadByte();
			this.Unused3 = reader.ReadByte();
		}

		public void encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(BlipTypeWin32);
			writer.Write(BlipTypeMacOS);
			writer.Write(UID.ToByteArray());
			writer.Write(Tag);
			writer.Write(BlipSize);
			writer.Write(Ref);
			writer.Write(Offset );
			writer.Write(Usage);
			writer.Write(NameLength);
			writer.Write(Unused2);
			writer.Write(Unused3);
			this.Data = stream.ToArray();
			this.Size = (UInt32)Data.Length;
			base.Encode();
		}

	}
}
