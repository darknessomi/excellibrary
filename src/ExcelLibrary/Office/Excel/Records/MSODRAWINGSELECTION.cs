using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MSODRAWINGSELECTION : MSOCONTAINER
	{
		public MSODRAWINGSELECTION(Record record) : base(record) { }

		public MSODRAWINGSELECTION()
		{
			this.Type = RecordType.MSODRAWINGSELECTION;
		}

	}
}
