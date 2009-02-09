using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtAlignRule : EscherRecord
	{
		public MsofbtAlignRule(EscherRecord record) : base(record) { }

		public MsofbtAlignRule()
		{
			this.Type = EscherRecordType.MsofbtAlignRule;
		}

	}
}
