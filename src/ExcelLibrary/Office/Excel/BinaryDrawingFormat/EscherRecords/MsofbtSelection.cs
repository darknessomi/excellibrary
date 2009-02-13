using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtSelection : EscherRecord
	{
		public MsofbtSelection(EscherRecord record) : base(record) { }

		public MsofbtSelection()
		{
			this.Type = EscherRecordType.MsofbtSelection;
		}

	}
}
