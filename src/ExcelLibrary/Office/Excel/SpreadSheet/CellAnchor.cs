using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.SpreadSheet
{
    public struct CellAnchor
    {
        public UInt16 RowIndex;

        public UInt16 ColIndex;

        public UInt16 DX;

        public UInt16 DY;

        public CellAnchor(UInt16 rowindex, UInt16 colindex, UInt16 dx, UInt16 dy)
        {
            RowIndex = rowindex;
            ColIndex = colindex;
            DX = dx;
            DY = dy;
        }
    }
}
