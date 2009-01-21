using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtCalloutRule : EscherRecord
	{
		public MsofbtCalloutRule(EscherRecord record) : base(record) { }

		public MsofbtCalloutRule()
		{
			this.Type = EscherRecordType.MsofbtCalloutRule;
		}

	}
}
