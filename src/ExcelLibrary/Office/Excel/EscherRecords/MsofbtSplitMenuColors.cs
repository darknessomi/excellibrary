using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtSplitMenuColors : EscherRecord
	{
		public MsofbtSplitMenuColors(EscherRecord record) : base(record) { }

		public MsofbtSplitMenuColors()
		{
			this.Type = EscherRecordType.MsofbtSplitMenuColors;
		}

	}
}
