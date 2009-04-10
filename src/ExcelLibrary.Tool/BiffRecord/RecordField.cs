using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ExcelLibrary.Tool
{
	public partial class RecordField
	{
		[XmlAttribute]
		public string Name;

		[XmlAttribute]
		public string Type;

		[XmlAttribute]
		public string ExtraInfo;

		[XmlElement]
		public string Description;

	}
}
