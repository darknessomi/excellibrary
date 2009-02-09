using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExcelLibrary.Office.Excel
{
	public partial class MsofbtOPT : EscherRecord
	{
        public List<ShapeProperty> Properties;

        public override void Decode()
        {
            Properties = new List<ShapeProperty>();

            for (int index = 0; index + 6 <= Data.Length; index += 6)
            {
                Properties.Add(ShapeProperty.Decode(Data, index));
            }
        }

    }
}
