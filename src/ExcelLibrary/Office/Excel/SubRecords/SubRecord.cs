using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class SubRecord
	{
		public static SubRecord Read(Stream stream)
		{
			SubRecord record = SubRecord.ReadBase(stream);
			switch (record.Type)
			{
				case SubRecordType.CommonObjectData:
					return new CommonObjectData(record);
				case SubRecordType.GroupMarker:
					return new GroupMarker(record);
				case SubRecordType.End:
					return new End(record);
				default:
					return record;
			}
		}

	}
}
