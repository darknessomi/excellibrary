using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// General   settings   for   the  document  window  and  global  workbook   settings
	/// </summary>
	public partial class WINDOW1 : Record
	{
		public WINDOW1(Record record) : base(record) { }

		public WINDOW1()
		{
			this.Type = RecordType.WINDOW1;
		}

		/// <summary>
		/// Horizontal position of the document window in twips
		/// </summary>
		public UInt16 HorizontalPosition;

		/// <summary>
		/// Vertical position of the document window in twips
		/// </summary>
		public UInt16 VerticalPosition;

		/// <summary>
		/// Width of the document window in twips
		/// </summary>
		public UInt16 WindowWidth;

		/// <summary>
		/// Height of the document window in twips
		/// </summary>
		public UInt16 WindowHeight;

		public UInt16 OptionFlags;

		/// <summary>
		/// Index to active (displayed) worksheet
		/// </summary>
		public UInt16 ActiveWorksheet;

		/// <summary>
		/// Index of first visible tab in the worksheet tab bar
		/// </summary>
		public UInt16 FirstVisibleTab;

		/// <summary>
		/// Number of selected worksheets (highlighted in the worksheet tab bar)
		/// </summary>
		public UInt16 SelecteWorksheets;

		/// <summary>
		/// Width of worksheet tab bar (in 1/1000 of window width).
		/// The remaining space is used by the horizontal scrollbar.
		/// </summary>
		public UInt16 TabBarWidth;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.HorizontalPosition = reader.ReadUInt16();
			this.VerticalPosition = reader.ReadUInt16();
			this.WindowWidth = reader.ReadUInt16();
			this.WindowHeight = reader.ReadUInt16();
			this.OptionFlags = reader.ReadUInt16();
			this.ActiveWorksheet = reader.ReadUInt16();
			this.FirstVisibleTab = reader.ReadUInt16();
			this.SelecteWorksheets = reader.ReadUInt16();
			this.TabBarWidth = reader.ReadUInt16();
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(HorizontalPosition);
			writer.Write(VerticalPosition);
			writer.Write(WindowWidth);
			writer.Write(WindowHeight);
			writer.Write(OptionFlags);
			writer.Write(ActiveWorksheet);
			writer.Write(FirstVisibleTab);
			writer.Write(SelecteWorksheets);
			writer.Write(TabBarWidth);
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
