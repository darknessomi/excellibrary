using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtArcRule : EscherRecord
	{
		public MsofbtArcRule(EscherRecord record) : base(record) { }

		public MsofbtArcRule()
		{
			this.Type = EscherRecordType.MsofbtArcRule;
		}

	}
}
