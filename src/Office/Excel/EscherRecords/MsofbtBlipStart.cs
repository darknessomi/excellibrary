using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtBlipStart : MsofbtBlip
	{
		public MsofbtBlipStart(EscherRecord record) : base(record) { }

		public MsofbtBlipStart()
		{
			this.Type = EscherRecordType.MsofbtBlipStart;
		}

	}
}
