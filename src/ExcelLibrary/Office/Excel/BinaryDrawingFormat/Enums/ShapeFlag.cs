using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.BinaryDrawingFormat
{
    public class ShapeFlag
    {
        public const int Group = 0x0001;
        public const int Child = 0x0002;
        public const int Patriarch = 0x0004;
        public const int Deleted = 0x0008;
        public const int Oleshape = 0x0010;
        public const int Havemaster = 0x0020;
        public const int Fliphoriz = 0x0040;
        public const int Flipvert = 0x0080;
        public const int Connector = 0x0100;
        public const int Haveanchor = 0x0200;
        public const int Background = 0x0400;
        public const int Hasshapetype = 0x0800;
    }
}
