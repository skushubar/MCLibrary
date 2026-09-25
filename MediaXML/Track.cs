using CsvHelper.Configuration.Attributes;

namespace MediaXML;

public class Track(int discId, short trackNumber, string name, int artistId, short rating, string filename, bool isStacked)
{
	public int DiscId { get; set; } = discId;

	public short TrackNumber { get; set; } = trackNumber;

	public string Name { get; set; } = name;

	public int ArtistId { get; set; } = artistId;

	public short Rating { get; set; } = rating;

	public string Filename { get; set; } = filename;

	[Ignore]
	public bool IsStacked { get; set; } = isStacked;

}
