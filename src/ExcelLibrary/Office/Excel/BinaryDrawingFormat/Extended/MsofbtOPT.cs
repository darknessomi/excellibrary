using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.BinaryDrawingFormat
{
    public partial class MsofbtOPT : EscherRecord
    {
        public List<ShapeProperty> Properties = new List<ShapeProperty>();

        public void Add(PropertyIDs propertyID, UInt32 propertyValue)
        {
            ShapeProperty prop = new ShapeProperty();
            prop.PropertyID = propertyID;
            prop.PropertyValue = propertyValue;
            prop.IsBlipID = propertyID == PropertyIDs.BlipId;
            Properties.Add(prop);
        }

        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
            BinaryReader reader = new BinaryReader(stream);
            Properties.Clear();
            for (int index = 0; index < this.Instance; index++)
            {
                Properties.Add(ShapeProperty.Decode(reader));
            }

            foreach (ShapeProperty property in Properties)
            {
                if (property.IsComplex)
                {
                    int size = (int)property.PropertyValue;
                    property.ComplexData = reader.ReadBytes(size);
                }
            }
        }

        public override void Encode()
        {
            this.Instance = (ushort)this.Properties.Count;
            this.Version = 3;
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            Properties.Sort(delegate(ShapeProperty p1, ShapeProperty p2)
            {
                return (int)p1.PropertyID - (int)p2.PropertyID;
            });
            foreach (ShapeProperty property in Properties)
            {
                property.Encode(writer);
            }
            foreach (ShapeProperty property in Properties)
            {
                if (property.IsComplex)
                {
                    writer.Write(property.ComplexData);
                }
            }
            this.Data = stream.ToArray();
            this.Size = (UInt32)Data.Length;
            base.Encode();
        }

    }
}
