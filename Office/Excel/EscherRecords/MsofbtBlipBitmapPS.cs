using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtBlipBitmapPS : MsofbtBlip
	{
		public MsofbtBlipBitmapPS(EscherRecord record) : base(record) { }

		public MsofbtBlipBitmapPS()
		{
			this.Type = EscherRecordType.MsofbtBlipBitmapPS;
		}

	}
}
