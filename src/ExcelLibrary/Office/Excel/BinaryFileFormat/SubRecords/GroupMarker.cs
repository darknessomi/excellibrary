using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
	public partial class GroupMarker : SubRecord
	{
		public GroupMarker(SubRecord record) : base(record) { }

		public GroupMarker()
		{
			this.Type = SubRecordType.GroupMarker;
		}

	}
}
