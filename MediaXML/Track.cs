namespace MediaXML;

public class Track(int discId, short trackNumber, string name, int artistId, short rating, string filename)
{
	public int DiscId { get; set; } = discId;

	public short TrackNumber { get; set; } = trackNumber;

	public string Name { get; set; } = name;

	public int ArtistId { get; set; } = artistId;

	public short Rating { get; set; } = rating;

	public string Filename { get; set; } = filename;

}
