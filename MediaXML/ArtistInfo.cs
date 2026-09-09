namespace MediaXML;

public class ArtistInfo
{
	public int nextArtistId { get; set; } = 1;

	public Dictionary<string, Artist> artistsDict { get; } = [];

	internal Artist GetOrCreateArtist(string artistName)
	{
		if (!artistsDict.TryGetValue(artistName, out var artist))
		{
			artist = new Artist(nextArtistId++, artistName);
			artistsDict[artistName] = artist;
		}
		return artist;
	}
}
