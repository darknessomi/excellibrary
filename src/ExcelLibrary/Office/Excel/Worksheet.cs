using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ExcelLibrary.CodeLib;

namespace ExcelLibrary.Office.Excel
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
        Dictionary<Pair<int, int>, Picture> images;
        public Dictionary<Pair<int, int>, Picture> Pictures
        {
            get
            {
                if (!extracted)
                {
                    images = ExtractPictures();
                    extracted = true;
                }
                return images;
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

        public Dictionary<Pair<int, int>, Picture> ExtractPictures()
        {
            Dictionary<Pair<int, int>, Picture> images = new Dictionary<Pair<int, int>, Picture>();
           
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

                                    Pair<int, int> cell = new Pair<int, int>(anchor.Row1, anchor.Col1);

                                    Picture pic = new Picture();
                                    pic.UpperRow = anchor.Row1;
                                    pic.BottomRow = anchor.Row2;
                                    pic.LeftCol = anchor.Col1;
                                    pic.RightCol = anchor.Col2;
                                    pic.ImageData = Book.ExtractImage(imageIndex, out pic.ImageFormat);
                                    images[cell] = pic;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return images;
        }
    }
}
