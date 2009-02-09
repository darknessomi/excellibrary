using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtBlipEnd : MsofbtBlip
	{
		public MsofbtBlipEnd(EscherRecord record) : base(record) { }

		public MsofbtBlipEnd()
		{
			this.Type = EscherRecordType.MsofbtBlipEnd;
		}

	}
}
