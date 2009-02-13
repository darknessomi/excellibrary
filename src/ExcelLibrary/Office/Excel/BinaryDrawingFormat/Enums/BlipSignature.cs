using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.BinaryDrawingFormat
{
    public static class BlipSignature
    {
        public const ushort UNKNOWN = 0;
        public const ushort EMF = 0x3D4;
        public const ushort WMF = 0x216;
        public const ushort PICT = 0x542;
        public const ushort JPEG = 0x46A;
        public const ushort PNG = 0x6E0;
        public const ushort JFIF = JPEG;
        public const ushort DIB = 0x7A8;
        public const ushort CMYKJPEG = 0x6E2;
        public const ushort TIFF = 0x6E4;
        public const ushort Client = 0x800;

        public static ushort FromBlipType(byte bliptype)
        {
            switch (bliptype)
            {
                case BlipType.EMF:
                    return BlipSignature.EMF;
                case BlipType.WMF:
                    return BlipSignature.WMF;
                case BlipType.PICT:
                    return BlipSignature.PICT;
                case BlipType.JPEG:
                    return BlipSignature.JPEG;
                case BlipType.PNG:
                    return BlipSignature.PNG;
                case BlipType.DIB:
                    return BlipSignature.DIB;
                default:
                    return BlipSignature.UNKNOWN;
            }
        }
    }
}
