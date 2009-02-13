using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
    /// <summary>
    /// Shape Properties msofbtOPT
    /// The first part of an OPT record is an array of FOPTEs,
    /// consisting of ID-value pairs 
    /// </summary>
    public class ShapeProperty
    {
        public PropertyIDs PropertyID;

        /// <summary>
        /// BLIP properties just store a BLIP ID (basically an index into an array in the BLIP Store).
        /// only valid if IsComplex is false
        /// </summary>
        public bool IsBlipID;

        /// <summary>
        /// if true, PropertyValue is the length of the data.
        /// The data of the complex properties follows the FOPTE array in the file record.
        /// </summary>
        public bool IsComplex;

        public UInt32 PropertyValue;

        public byte[] ComplexData;

        public const int Size = 6;

        public static ShapeProperty Decode(BinaryReader reader)
        {
            ShapeProperty property = new ShapeProperty();
            UInt16 num = reader.ReadUInt16();
            property.PropertyID = (PropertyIDs)(num & 0x3FFF);
            property.IsBlipID = (num & 0x4000) == 0x4000;
            property.IsComplex = (num & 0x8000) == 0x8000;
            property.PropertyValue = reader.ReadUInt32();
            return property;
        }

        public void Encode(BinaryWriter writer)
        {
            UInt16 num = (UInt16)((UInt16)PropertyID & 0x3FFF);
            if (IsBlipID)
            {
                num = (UInt16)(num | 0x4000);
            }
            if (IsComplex)
            {
                num = (UInt16)(num | 0x8000);
            }
            writer.Write(num);
            writer.Write(PropertyValue);
        }
    }

}
