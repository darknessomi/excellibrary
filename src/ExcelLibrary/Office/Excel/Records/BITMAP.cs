using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class BITMAP : Record
	{
		public BITMAP(Record record) : base(record) { }

		public BITMAP()
		{
			this.Type = RecordType.BITMAP;
		}

	}
}
