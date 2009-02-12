using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtBlip : EscherRecord
	{
		public MsofbtBlip() { }

		public MsofbtBlip(EscherRecord record) : base(record) { }

		public Guid UID;

		public Byte Marker;

		public Byte[] ImageData;

	}
}
