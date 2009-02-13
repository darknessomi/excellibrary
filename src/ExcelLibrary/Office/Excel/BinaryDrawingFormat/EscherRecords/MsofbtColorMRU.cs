using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtColorMRU : EscherRecord
	{
		public MsofbtColorMRU(EscherRecord record) : base(record) { }

		public MsofbtColorMRU()
		{
			this.Type = EscherRecordType.MsofbtColorMRU;
		}

	}
}
