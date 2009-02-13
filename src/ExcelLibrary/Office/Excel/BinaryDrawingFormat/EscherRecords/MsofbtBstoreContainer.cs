using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtBstoreContainer : MsofbtContainer
	{
		public MsofbtBstoreContainer(EscherRecord record) : base(record) { }

		public MsofbtBstoreContainer()
		{
			this.Type = EscherRecordType.MsofbtBstoreContainer;
		}

	}
}
