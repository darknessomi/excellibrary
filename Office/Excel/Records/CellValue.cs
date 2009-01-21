using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class CellValue : Record
	{
		public CellValue() { }

		public CellValue(Record record) : base(record) { }

		/// <summary>
		/// Index to row
		/// </summary>
		public UInt16 RowIndex;

		/// <summary>
		/// Index to column
		/// </summary>
		public UInt16 ColIndex;

		/// <summary>
		/// Index to XF record
		/// </summary>
		public UInt16 XFIndex;

	}
}
