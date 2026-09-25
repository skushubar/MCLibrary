namespace MediaXML;

internal class Builder(MPL mpl)
{
	private MPL mpl = mpl;

	private readonly Dictionary<AlbumKey, Album> albumsDict = [];
	private readonly ArtistInfo artistInfo = new();
	private readonly List<Album> stackedAlbums = [];

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
			CheckIfStacked(track, album);
		}

		foreach (var album in albumsDict.Values)
		{
			album.CleanMultipleArtists(artistInfo);
		}

		if (stackedAlbums.Count > 0)
		{
			ErrorLogger.LogError($"The following {stackedAlbums.Count} albums have stacked tracks:");

			var stackedByArtist = stackedAlbums.GroupBy(a => a.Artist.ToString()).OrderBy(g => g.Key);
			foreach (var artistGroup in stackedByArtist)
			{
				//ErrorLogger.LogError($"- {artistGroup.Key}:");
				foreach (var album in artistGroup.OrderBy(a => a.Title))
				{
					ErrorLogger.LogError($"- {album}");
				}
			}

			//foreach (var album in stackedAlbums)
			//{
			//	ErrorLogger.LogError($"- {album})");
			//}
			ErrorLogger.LogError($"{stackedAlbums.Count} albums have stacked tracks.");
			Console.WriteLine();
		}

		return true;
	}

	private void CheckIfStacked(Track track, Album album)
	{
		if (!track.IsStacked) return;
		if (!stackedAlbums.Contains(album)) stackedAlbums.Add(album);
	}

	private Album GetOrCreateAlbum(FieldDictionary fields)
	{
		var albumTitle = fields.GetStringNotEmpty("Album");
		string albumDate = fields.GetString("Date (readable)");
		var albumKey = new AlbumKey(albumTitle, albumDate);
		if (!albumsDict.TryGetValue(albumKey, out var album))
		{
			var albumGenre = fields.GetString("Genre");
			var (artist, isAlbumArtist) = GetOrCreateArtist(fields);
			album = new Album(nextAlbumId++, artist, isAlbumArtist, albumTitle, albumDate, albumGenre);
			albumsDict[albumKey] = album;
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
