using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ExcelLibrary.BinaryDrawingFormat;

namespace ExcelLibrary.SpreadSheet
{
    public class Image
    {
        public ushort Format;
        public byte[] Data;

        public Image(byte[] data, ushort imageFormat)
        {
            Data = data;
            Format = imageFormat;
        }

        public string FileExtension
        {
            get { return GetImageFileExtension(this.Format); }
        }

        public static Image FromFile(string filepath)
        {
            byte[] data = File.ReadAllBytes(filepath);
            ushort format = JudgeFromFileExtension(Path.GetExtension(filepath));
            return new Image(data, format);
        }

        public static string GetImageFileExtension(ushort imageForamt)
        {
            switch (imageForamt)
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
