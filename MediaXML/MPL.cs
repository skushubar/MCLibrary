using System.Xml.Serialization;

namespace MediaXML;

public class MPL
{
	[XmlAttribute("Version")]
	public string Version { get; set; } = string.Empty;
	[XmlAttribute("Title")]
	public string Title { get; set; } = string.Empty;

	[XmlElement("Item")]
	public List<MPLItem> Items { get; set; } = [];

	public override string ToString()
	{
		return $"Version: {Version}, Title: {Title}, ItemCount: {Items.Count}";
	}
}
