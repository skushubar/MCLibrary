using System.Xml.Serialization;

namespace MediaXML;

public class MPLItem
{

	[XmlElement("Field")]
	public List<MPLField> Fields { get; set; } = [];

	public override string ToString()
	{
		return $"FieldCount: {Fields.Count}";
	}
}
