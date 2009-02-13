using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.BinaryFileFormat
{
    public class SubRecordType
    {
        public const UInt16 End = 0x0000;
        public const UInt16 GroupMarker = 0x0006;
        public const UInt16 ClipboardFormat = 0x0007;
        public const UInt16 PictureOption = 0x0008;
        public const UInt16 CommonObjectData = 0x0015;
    }
}
