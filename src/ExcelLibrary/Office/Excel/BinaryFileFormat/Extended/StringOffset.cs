using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.BinaryFileFormat
{
    public class StringOffset
    {
        /// <summary>
        /// Absolute stream position of first string of the portion
        /// </summary>
        public UInt32 AbsolutePosition;

        /// <summary>
        /// Relative record position of string
        /// </summary>
        public UInt16 RelativePosition;

        public UInt16 NotUsed;
    }
}
