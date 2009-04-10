using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ExcelLibrary.Tool
{
	public partial class Record
	{
		[XmlAttribute]
		public string Name;

		[XmlAttribute]
		public string Parent;

		[XmlAttribute]
		public string Category;

		[XmlElement]
		public bool IsAbstract;

		[XmlElement]
		public bool IsCutomized;

		[XmlElement]
		public string Description;

		[XmlArray]
		public List<RecordField> Fields;

		[XmlElement("Record")]
		public List<Record> ChildRecords;

	}
}
