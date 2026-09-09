using System.Xml.Serialization;

namespace MediaXML;

public class MPLField
{
	[XmlAttribute("Name")]
	public string Name { get; set; } = null!;

	[XmlText]
	public string Value { get; set; } = null!;

	public override string ToString()
	{
		return $"Name: {Name}, Value: {Value}";
	}
}
