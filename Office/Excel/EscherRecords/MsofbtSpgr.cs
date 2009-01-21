using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtSpgr : EscherRecord
	{
		public MsofbtSpgr(EscherRecord record) : base(record) { }

		public MsofbtSpgr()
		{
			this.Type = EscherRecordType.MsofbtSpgr;
		}

	}
}
