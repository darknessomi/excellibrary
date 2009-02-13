using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;
using ExcelLibrary.BinaryFileFormat;
using ExcelLibrary.BinaryDrawingFormat;

namespace ExcelLibrary.SpreadSheet
{
    public class Worksheet
    {
        public Workbook Book;

        public string Name;
        public SheetType SheetType;
        public MSODRAWING Drawing;

        public CellCollection Cells;

        internal Worksheet()
        {
        }

        public Worksheet(string name)
        {
            this.Name = name;
            this.Cells = new CellCollection();
        }

        bool extracted = false;
        Dictionary<Pair<int, int>, Picture> pictures = new Dictionary<Pair<int, int>, Picture>();
        public Dictionary<Pair<int, int>, Picture> Pictures
        {
            get
            {
                if (!extracted)
                {
                    ExtractPictures();
                    extracted = true;
                }
                return pictures;
            }
        }

        public Picture ExtractPicture(int row, int col)
        {
            Pair<int, int> pos = new Pair<int, int>(row, col);
            if (Pictures.ContainsKey(pos))
            {
                return Pictures[pos];
            }
            else
            {
                return null;
            }
        }

        public void AddPicture(Picture pic)
        {
            pictures[pic.CellPos] = pic;
        }

        public void ExtractPictures()
        {           
            if (Drawing != null)
            {
                MsofbtDgContainer dgContainer = Drawing.FindChild<MsofbtDgContainer>();
                if (dgContainer != null)
                {
                    MsofbtSpgrContainer spgrContainer = dgContainer.FindChild<MsofbtSpgrContainer>();

                    List<MsofbtSpContainer> spContainers = spgrContainer.FindChildren<MsofbtSpContainer>();

                    foreach (MsofbtSpContainer spContainer in spContainers)
                    {
                        MsofbtOPT opt = spContainer.FindChild<MsofbtOPT>();
                        MsofbtClientAnchor anchor = spContainer.FindChild<MsofbtClientAnchor>();

                        if (opt != null && anchor != null)
                        {
                            foreach (ShapeProperty prop in opt.Properties)
                            {
                                if (prop.PropertyID == PropertyIDs.BlipId)
                                {
                                    int imageIndex = (int)prop.PropertyValue - 1;
                                    Picture pic = new Picture();
                                    pic.TopLeftCorner.RowIndex = anchor.Row1;
                                    pic.TopLeftCorner.ColIndex = anchor.Col1;
                                    pic.TopLeftCorner.DX = anchor.DX1;
                                    pic.TopLeftCorner.DY = anchor.DY1;
                                    pic.BottomRightCorner.RowIndex = anchor.Row2;
                                    pic.BottomRightCorner.ColIndex = anchor.Col2;
                                    pic.BottomRightCorner.DX = anchor.DX2;
                                    pic.BottomRightCorner.DY = anchor.DY2;
                                    pic.Image = Book.ExtractImage(imageIndex);
                                    pictures[pic.CellPos] = pic;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
