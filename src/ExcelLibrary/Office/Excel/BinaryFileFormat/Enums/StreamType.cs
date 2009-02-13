using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.BinaryFileFormat
{
    public class StreamType
    {
        public const UInt16 WorkbookGlobals = 0x0005;
        public const UInt16 VisualBasicModule = 0x0006;
        public const UInt16 Worksheet = 0x0010;
        public const UInt16 Chart = 0x0020;
        public const UInt16 Macrosheet = 0x0040;
        public const UInt16 WorkspaceFile = 0x0100;
    }
}
