using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtBlipBitmapDIB : MsofbtBlip
	{
		public MsofbtBlipBitmapDIB(EscherRecord record) : base(record) { }

		public MsofbtBlipBitmapDIB()
		{
			this.Type = EscherRecordType.MsofbtBlipBitmapDIB;
		}

	}
}
