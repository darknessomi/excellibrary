using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtClientRule : EscherRecord
	{
		public MsofbtClientRule(EscherRecord record) : base(record) { }

		public MsofbtClientRule()
		{
			this.Type = EscherRecordType.MsofbtClientRule;
		}

	}
}
