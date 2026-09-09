using CsvHelper.Configuration.Attributes;

namespace MediaXML;

public class Disc(Album album, short discNumber)
{
	private static int nextId = 1;

	public int Id { get; } = nextId++;
	public int AlbumId => Album.Id;
	public short DiscNumber { get; set; } = discNumber;

	[Ignore]
	public Album Album { get; } = album;

	public List<Track> Tracks { get; set; } = [];

	internal Track AddTrack(FieldDictionary fields, ArtistInfo artistInfo)
	{
		var name = fields.GetStringNotEmpty("Name");
		Artist artist = GetOrCreateArtist(fields, artistInfo);
		var rating = fields.GetShort("Rating");
		if (rating is null)
			ErrorLogger.LogError($"Rating is missing for track '{name}' on disc {DiscNumber} of album {Album}");
		var trackNumber = fields.GetShort("Track #");
		if (trackNumber is null)
			ErrorLogger.LogError($"Track number  is missing for track '{name}' on disc {DiscNumber} of album {Album}");
		Track track = new(Id, trackNumber ?? -1, name, artist.Id, rating ?? -1, fields.GetStringNotEmpty("Filename"));
		Tracks.Add(track);
		return track;
	}

	private static Artist GetOrCreateArtist(FieldDictionary fields, ArtistInfo artistInfo)
	{
		var artistName = fields.GetStringNotEmpty("Artist");
		return artistInfo.GetOrCreateArtist(artistName);
	}

}
