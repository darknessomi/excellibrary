using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtOleObject : EscherRecord
	{
		public MsofbtOleObject(EscherRecord record) : base(record) { }

		public MsofbtOleObject()
		{
			this.Type = EscherRecordType.MsofbtOleObject;
		}

	}
}
