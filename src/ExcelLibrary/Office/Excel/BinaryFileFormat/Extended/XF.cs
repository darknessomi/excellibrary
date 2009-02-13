using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryFileFormat
{
    public partial class XF : Record
    {
        public int PatternColorIndex
        {
            get { return Background & 0x007F; }
            set
            {
                if (value > 0x7F)
                {
                    throw new ArgumentOutOfRangeException("PatternColorIndex");
                }
                Background = (ushort)(Background & 0x3F80 | value);
            }
        }

        public int PatternBackgroundColorIndex
        {
            get { return (Background & 0x3F80) >> 6; }
            set
            {
                Background = (ushort)(Background & 0x007F | value << 6);
            }
        }
    }
}
