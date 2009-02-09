using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtBlipBitmapJPEG : MsofbtBlip
	{
		public MsofbtBlipBitmapJPEG(EscherRecord record) : base(record) { }

		public MsofbtBlipBitmapJPEG()
		{
			this.Type = EscherRecordType.MsofbtBlipBitmapJPEG;
		}

	}
}
