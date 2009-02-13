using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
    public partial class BOOLERR : CellValue
	{
        public object GetValue()
        {
            if (ValueType == 0) // Boolean value
            {
                return Value == 1;
            }
            else // Error code
            {
                return ErrorCode.ErrorCodes[Value];
            }
        }
	}
}
