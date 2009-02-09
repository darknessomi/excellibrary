using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MSODRAWING : MSOCONTAINER
	{
		public MSODRAWING(Record record) : base(record) { }

		public MSODRAWING()
		{
			this.Type = RecordType.MSODRAWING;
		}

	}
}
