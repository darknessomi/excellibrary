using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
    public partial class FORMULA : Record
    {
        public STRING StringRecord;

        public object DecodeResult()
        {
            byte[] bytes = BitConverter.GetBytes(Result);
            if (bytes[6] == 0xFF && bytes[7] == 0xFF)
            {
                switch (bytes[0])
                {
                    case 0: //string value
                        if (StringRecord != null)
                        {
                            return StringRecord.Value;
                        }
                        else
                        {
                            break;
                        }
                    case 1: //Boolean value
                        return Convert.ToBoolean(bytes[2]);
                    case 2: //error value
                        return ErrorCode.ErrorCodes[bytes[2]];
                    case 3: //empty cell
                        return string.Empty;
                }
            }
            return BitConverter.ToDouble(bytes, 0);
        }
    }
}
