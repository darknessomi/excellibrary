using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelLibrary.BinaryDrawingFormat
{
    public static class BlipType
    {
        public const byte ERROR = 0;
        public const byte UNKNOWN = 1;
        public const byte EMF = 2;
        public const byte WMF = 3;
        public const byte PICT = 4;
        public const byte JPEG = 5;
        public const byte PNG = 6;
        public const byte DIB = 7;

        public static byte FromImageFormat(ushort imageForamt)
        {
            switch (imageForamt)
            {
                case EscherRecordType.MsofbtBlipMetafileEMF:
                    return BlipType.EMF;
                case EscherRecordType.MsofbtBlipMetafileWMF:
                    return BlipType.WMF;
                case EscherRecordType.MsofbtBlipMetafilePICT:
                    return BlipType.PICT;
                case EscherRecordType.MsofbtBlipBitmapJPEG:
                    return BlipType.JPEG;
                case EscherRecordType.MsofbtBlipBitmapPNG:
                    return BlipType.PNG;
                case EscherRecordType.MsofbtBlipBitmapDIB:
                    return BlipType.DIB;
                default:
                    return BlipType.UNKNOWN;
            }
        }
    }
}
