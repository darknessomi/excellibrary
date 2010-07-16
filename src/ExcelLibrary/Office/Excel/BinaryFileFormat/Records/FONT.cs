using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class FONT : Record
	{
		public FONT(Record record) : base(record) { }

		public FONT()
		{
			this.Type = RecordType.FONT;
		}

		/// <summary>
		/// Height of the font (in twips = 1/20 of a point)
		/// </summary>
		public Int16 Height;

		/// <summary>
		/// Option flags
		/// </summary>
		public UInt16 OptionFlags;

		/// <summary>
		/// Colour index
		/// </summary>
		public UInt16 ColorIndex;

		/// <summary>
		/// Font weight (100-1000). Standard values are 0190H (400) for normal text and 02BCH
		/// (700) for bold text.
		/// </summary>
		public UInt16 Weight;

		/// <summary>
		/// Escapement type: 0000H = None
		/// 0001H = Superscript
		/// 0002H = Subscript
		/// </summary>
		public UInt16 Escapement;

		/// <summary>
		/// Underline type: 00H = None
		/// 01H = Single 21H = Single accounting
		/// 02H = Double 22H = Double accounting
		/// </summary>
		public Byte Underline;

		/// <summary>
		/// Font family: 00H = None (unknown or don't care)
		/// 01H = Roman (variable width, serifed)
		/// 02H = Swiss (variable width, sans-serifed)
		/// 03H = Modern (fixed width, serifed or sans-serifed)
		/// 04H = Script (cursive)
		/// 05H = Decorative (specialised, for example Old English, Fraktur)
		/// </summary>
		public Byte Family;

		/// <summary>
		/// Character set (used by all cell records containing byte strings)
		/// </summary>
		public Byte CharacterSet;

		public Byte NotUsed;

		/// <summary>
		/// Font name
		/// </summary>
		public String Name;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Height = reader.ReadInt16();
			this.OptionFlags = reader.ReadUInt16();
			this.ColorIndex = reader.ReadUInt16();
			this.Weight = reader.ReadUInt16();
			this.Escapement = reader.ReadUInt16();
			this.Underline = reader.ReadByte();
			this.Family = reader.ReadByte();
			this.CharacterSet = reader.ReadByte();
			this.NotUsed = reader.ReadByte();
			this.Name = this.ReadString(reader, 8);
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(Height);
			writer.Write(OptionFlags);
			writer.Write(ColorIndex);
			writer.Write(Weight);
			writer.Write(Escapement);
			writer.Write(Underline);
			writer.Write(Family);
			writer.Write(CharacterSet);
			writer.Write(NotUsed);
			Record.WriteString(writer, Name, 8);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
