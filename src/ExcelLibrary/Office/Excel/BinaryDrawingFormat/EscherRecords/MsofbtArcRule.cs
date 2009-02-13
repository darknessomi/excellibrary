using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
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
