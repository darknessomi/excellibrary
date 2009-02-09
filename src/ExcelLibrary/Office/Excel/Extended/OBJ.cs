using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class OBJ : Record
	{
        public List<SubRecord> SubRecords;

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
            SubRecords = new List<SubRecord>();
            while (stream.Position < Size)
            {
                SubRecords.Add(SubRecord.Read(stream));
            }
        }

	}
}
