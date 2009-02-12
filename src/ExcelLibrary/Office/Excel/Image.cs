using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
    public class Image
    {
        public ushort Format;
        public byte[] Data;

        public Image(byte[] data, ushort format)
        {
            Data = data;
            Format = format;
        }

        public string FileExtension
        {
            get { return GetImageFileExtension(this.Format); }
        }

        public byte BlipType
        {
            get { return ExcelLibrary.Office.Excel.BlipType.FromImageType(this.Format); }
        }

        public static Image FromFile(string filepath)
        {
            byte[] data = File.ReadAllBytes(filepath);
            ushort format = JudgeFromFileExtension(Path.GetExtension(filepath));
            return new Image(data, format);
        }

        public static string GetImageFileExtension(ushort imagetype)
        {
            switch (imagetype)
            {
                case EscherRecordType.MsofbtBlipMetafileEMF:
                    return ".emf";
                case EscherRecordType.MsofbtBlipMetafileWMF:
                    return ".wmf";
                case EscherRecordType.MsofbtBlipBitmapJPEG:
                    return ".jpeg";
                case EscherRecordType.MsofbtBlipBitmapPNG:
                    return ".png";
                case EscherRecordType.MsofbtBlipBitmapDIB:
                    return ".bmp";
                default:
                    return "unknown";
            }
        }

        private static ushort JudgeFromFileExtension(string ext)
        {
            switch (ext.ToLower())
            {
                case ".emf":
                    return EscherRecordType.MsofbtBlipMetafileEMF;
                case ".wmf":
                    return EscherRecordType.MsofbtBlipMetafileWMF;
                case ".jpeg":
                    return EscherRecordType.MsofbtBlipBitmapJPEG;
                case ".png":
                    return EscherRecordType.MsofbtBlipBitmapPNG;
                case ".bmp":
                    return EscherRecordType.MsofbtBlipBitmapDIB;
                default:
                    return 0;
            }
        }
    }
}
