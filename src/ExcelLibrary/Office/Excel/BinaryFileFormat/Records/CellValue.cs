using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class CellValue : Record
	{
		public CellValue() { }

		public CellValue(Record record) : base(record) { }

		/// <summary>
		/// Index to row, 0-based row number
		/// </summary>
		public UInt16 RowIndex;

		/// <summary>
		/// Index to column, 0-based column number
		/// </summary>
		public UInt16 ColIndex;

		/// <summary>
		/// Index to XF record
		/// </summary>
		public UInt16 XFIndex;

	}
}
