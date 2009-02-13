using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public class ByteOrderMarks
    {
        public static readonly byte[] LittleEndian = new byte[] { 0xFE, 0xFF };
        public static readonly byte[] BigEndian = new byte[] { 0xFF, 0xFE };
    }
}
