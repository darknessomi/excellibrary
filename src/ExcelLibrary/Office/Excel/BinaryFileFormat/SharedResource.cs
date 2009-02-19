using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;
using ExcelLibrary.SpreadSheet;

namespace ExcelLibrary.BinaryFileFormat
{
    public class SharedResource
    {
        public SST SharedStringTable;

        public DateTime BaseDate;

        public ColorPalette ColorPalette = new ColorPalette();

        public List<FORMAT> FormatRecords = new List<FORMAT>();

        public List<XF> ExtendedFormats = new List<XF>();

        public CellFormatCollection CellFormats = new CellFormatCollection();

        public UniqueList<Image> Images = new UniqueList<Image>();

        public List<FONT> Fonts = new List<FONT>();

        public SharedResource()
        {
        }

        public SharedResource(bool newbook)
        {
            FONT font = new FONT();
            font.Height = 200;
            font.OptionFlags = 0;
            font.ColorIndex = 32767;
            font.Weight = 400;
            font.Escapement = 0;
            font.Underline = 0;
            font.CharacterSet = 1;
            font.Name = "Arial";
            //Fonts.Add(font);

            for (ushort i = 0; i < 21; i++) // required by MS Excel 2003
            {
                XF xf = new XF();
                xf.Attributes = 252;
                xf.CellProtection = 65524;
                xf.PatternColorIndex = 64;
                xf.PatternBackgroundColorIndex = 130;
                xf.FontIndex = 0;
                xf.FormatIndex = i;
                ExtendedFormats.Add(xf);
            }

            MaxNumberFormatIndex = 163;
            GetXFIndex(CellFormat.General);

            SharedStringTable = new SST();
        }

        public string GetStringFromSST(int index)
        {
            if (SharedStringTable != null)
            {
                return SharedStringTable.StringList[index];
            }
            return null;
        }

        public int GetSSTIndex(string text)
        {
            SharedStringTable.TotalOccurance++;
            int index = SharedStringTable.StringList.IndexOf(text);
            if (index == -1)
            {
                SharedStringTable.StringList.Add(text);
                return SharedStringTable.StringList.Count - 1;
            }
            else
            {
                return index;
            }
        }

        public double EncodeDateTime(DateTime value)
        {
            double days = (value - BaseDate).Days;
            if (days > 365) days++;
            return days;
        }

        Dictionary<string, int> NumberFormatXFIndice = new Dictionary<string, int>();
        ushort MaxNumberFormatIndex;
        internal int GetXFIndex(CellFormat cellFormat)
        {
            string formatString = cellFormat.FormatString;
            if (NumberFormatXFIndice.ContainsKey(formatString))
            {
                return NumberFormatXFIndice[formatString];
            }
            else
            {
                UInt16 formatIndex = CellFormats.GetFormatIndex(formatString);
                if (formatIndex == UInt16.MaxValue)
                {
                    formatIndex = MaxNumberFormatIndex++;
                }

                FORMAT format = new FORMAT();
                format.FormatIndex = formatIndex;
                format.FormatString = formatString;
                FormatRecords.Add(format);

                XF xf = new XF();
                xf.Attributes = 252;
                xf.CellProtection = 0;
                xf.PatternColorIndex = 64;
                xf.PatternBackgroundColorIndex = 130;
                xf.FontIndex = 0;
                xf.FormatIndex = formatIndex;
                ExtendedFormats.Add(xf);

                int numberFormatXFIndex = ExtendedFormats.Count - 1;
                NumberFormatXFIndice.Add(formatString, numberFormatXFIndex);

                return numberFormatXFIndex;
            }
        }
    }
}
