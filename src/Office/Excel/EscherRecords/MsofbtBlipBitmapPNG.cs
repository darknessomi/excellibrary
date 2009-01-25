using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtBlipBitmapPNG : MsofbtBlip
	{
		public MsofbtBlipBitmapPNG(EscherRecord record) : base(record) { }

		public MsofbtBlipBitmapPNG()
		{
			this.Type = EscherRecordType.MsofbtBlipBitmapPNG;
		}

	}
}
