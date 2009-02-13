using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	/// <summary>
	/// Indicates the end of workbook globals, a worksheet, a chart, etc.
	/// </summary>
	public partial class EOF : Record
	{
		public EOF(Record record) : base(record) { }

		public EOF()
		{
			this.Type = RecordType.EOF;
		}

	}
}
