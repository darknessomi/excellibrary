using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtCLSID : EscherRecord
	{
		public MsofbtCLSID(EscherRecord record) : base(record) { }

		public MsofbtCLSID()
		{
			this.Type = EscherRecordType.MsofbtCLSID;
		}

	}
}
