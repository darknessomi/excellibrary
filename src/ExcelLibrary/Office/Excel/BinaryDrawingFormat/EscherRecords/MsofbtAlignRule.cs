using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
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
