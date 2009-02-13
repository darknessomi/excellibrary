using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtContainer : EscherRecord
	{
		public MsofbtContainer() { }

		public MsofbtContainer(EscherRecord record) : base(record) { }

	}
}
