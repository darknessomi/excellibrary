using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtSpContainer : MsofbtContainer
	{
		public MsofbtSpContainer(EscherRecord record) : base(record) { }

		public MsofbtSpContainer()
		{
			this.Type = EscherRecordType.MsofbtSpContainer;
		}

	}
}
