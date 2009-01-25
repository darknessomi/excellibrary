using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.Office.Excel
{
    public class Picture
    {
        public int LeftCol;
        public int RightCol;
        public int UpperRow;
        public int BottomRow;
        public ushort ImageFormat;
        public byte[] ImageData;
    }
}
