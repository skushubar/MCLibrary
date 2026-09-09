namespace MediaXML;

internal class Builder(MPL mpl)
{
	private MPL mpl = mpl;

	private Dictionary<string, Album> albumsDict = [];
	private ArtistInfo artistInfo = new();

	private int nextAlbumId = 1;

	public IReadOnlyList<Album> Albums => [.. albumsDict.Values];
	public IReadOnlyList<Artist> Artists => [.. artistInfo.artistsDict.Values];

	public bool Build()
	{
		foreach (var item in mpl.Items)
		{
			FieldDictionary fields = new(item.Fields.ToDictionary(f => f.Name, f => f.Value));
			var mediaType = fields.GetStringNotEmpty("Media Type");
			if (mediaType == "Video") continue;
			var album = GetOrCreateAlbum(fields);
			Disc disc = album.GetOrCreateDisc(fields);
			Track track = disc.AddTrack(fields, artistInfo);
		}

		foreach (var album in albumsDict.Values)
		{
			album.CleanMultipleArtists(artistInfo);
		}

		return true;
	}

	private Album GetOrCreateAlbum(FieldDictionary fields)
	{
		var albumTitle = fields.GetStringNotEmpty("Album");
		if (!albumsDict.TryGetValue(albumTitle, out var album))
		{
			string albumDate = fields.GetString("Date (readable)");
			var albumGenre = fields.GetString("Genre");
			var (artist, isAlbumArtist) = GetOrCreateArtist(fields);
			album = new Album(nextAlbumId++, artist, isAlbumArtist, albumTitle, albumDate, albumGenre);
			albumsDict[albumTitle] = album;
		}
		return album;
	}

	private (Artist artist, bool albumArtist) GetOrCreateArtist(FieldDictionary fields)
	{
		var artistName = fields.GetString("Album Artist");
		if (!string.IsNullOrEmpty(artistName))
		{
			return (artistInfo.GetOrCreateArtist(artistName), true);
		}
		artistName = fields.GetStringNotEmpty("Artist");
		return (artistInfo.GetOrCreateArtist(artistName), false);
	}
}
