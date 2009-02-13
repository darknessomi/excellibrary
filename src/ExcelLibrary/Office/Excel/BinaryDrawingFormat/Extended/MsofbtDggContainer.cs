using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
    public partial class MsofbtDggContainer
	{
        public MsofbtBstoreContainer BstoreContainer
        {
            get
            {
                foreach (EscherRecord record in EscherRecords)
                {
                    if (record.Type == EscherRecordType.MsofbtBstoreContainer)
                    {
                        return record as MsofbtBstoreContainer;
                    }
                }
                return null;
            }
        }
	}
}
