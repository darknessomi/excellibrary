using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtTextbox : EscherRecord
	{
		public MsofbtTextbox(EscherRecord record) : base(record) { }

		public MsofbtTextbox()
		{
			this.Type = EscherRecordType.MsofbtTextbox;
		}

	}
}
