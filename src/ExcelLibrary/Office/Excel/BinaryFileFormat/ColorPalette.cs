using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ExcelLibrary.BinaryFileFormat
{
    public class ColorPalette
    {
        public Dictionary<int, Color> Palette = new Dictionary<int, Color>();

        public ColorPalette()
        {
            Palette.Add(0, Color.Black);
            Palette.Add(1, Color.White);
            Palette.Add(2, Color.Red);
            Palette.Add(3, Color.Green);
            Palette.Add(4, Color.Blue);
            Palette.Add(5, Color.Yellow);
            Palette.Add(6, Color.Magenta);
            Palette.Add(7, Color.Cyan);
            // 0x08-0x3F: default color table
            Palette.Add(0x08, Color.FromArgb(0, 0, 0));
            Palette.Add(0x09, Color.FromArgb(0xFF, 0xFF, 0xFF));
            Palette.Add(0x0A, Color.FromArgb(0xFF, 0, 0));

            Palette.Add(0x1F, Color.FromArgb(0xCC, 0xCC, 0xFF));

            Palette.Add(0x38, Color.FromArgb(0x00, 0x33, 0x66));
            Palette.Add(0x39, Color.FromArgb(0x33, 0x99, 0x66));
            Palette.Add(0x3A, Color.FromArgb(0x00, 0x33, 0x00));
            Palette.Add(0x3B, Color.FromArgb(0x33, 0x33, 0x00));
            Palette.Add(0x3C, Color.FromArgb(0x99, 0x33, 0x00));
            Palette.Add(0x3D, Color.FromArgb(0x99, 0x33, 0x66));
            Palette.Add(0x3E, Color.FromArgb(0x33, 0x33, 0x99));
            Palette.Add(0x3F, Color.FromArgb(0x33, 0x33, 0x33));

            Palette.Add(0x40, SystemColors.Window);
            Palette.Add(0x41, SystemColors.WindowText);
            Palette.Add(0x43, SystemColors.WindowFrame);//dialogue background colour
            Palette.Add(0x4D, SystemColors.ControlText);//text colour for chart border lines
            Palette.Add(0x4E, SystemColors.Control); //background colour for chart areas
            Palette.Add(0x4F, Color.Black); //Automatic colour for chart border lines
            Palette.Add(0x50, SystemColors.Info);
            Palette.Add(0x51, SystemColors.InfoText);
            Palette.Add(0x7FFF, SystemColors.WindowText);
        }

        public Color this[int index]
        {
            get
            {
                if (Palette.ContainsKey(index))
                {
                    return Palette[index];
                }
                return Color.White;
            }
            set
            {
                Palette[index] = value;
            }
        }
    }
}
