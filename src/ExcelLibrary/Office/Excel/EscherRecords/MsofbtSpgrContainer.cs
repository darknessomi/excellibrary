using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtSpgrContainer : MsofbtContainer
	{
		public MsofbtSpgrContainer(EscherRecord record) : base(record) { }

		public MsofbtSpgrContainer()
		{
			this.Type = EscherRecordType.MsofbtSpgrContainer;
		}

	}
}
