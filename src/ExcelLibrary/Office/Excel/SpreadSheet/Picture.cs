using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace ExcelLibrary.SpreadSheet
{
    public class Picture
    {
        public CellAnchor TopLeftCorner;
        public CellAnchor BottomRightCorner;
        public Image Image;

        public Pair<int, int> CellPos
        {
            get
            {
                return new Pair<int, int>(TopLeftCorner.RowIndex, TopLeftCorner.ColIndex);
            }
        }
    }
}
