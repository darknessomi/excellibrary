using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class CONTINUE : Record
	{
		public CONTINUE(Record record) : base(record) { }

		public CONTINUE()
		{
			this.Type = RecordType.CONTINUE;
		}

	}
}
