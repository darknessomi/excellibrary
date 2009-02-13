using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtSolverContainer : MsofbtContainer
	{
		public MsofbtSolverContainer(EscherRecord record) : base(record) { }

		public MsofbtSolverContainer()
		{
			this.Type = EscherRecordType.MsofbtSolverContainer;
		}

	}
}
