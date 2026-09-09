using CsvHelper.Configuration.Attributes;

namespace MediaXML;

public class Album(int id, Artist artist, bool isAlbumArtist, string title, string date, string genre)
{
	public int Id { get; set; } = id;
	public int ArtistId => Artist.Id;
	public bool AlbumArtistSpecified => isAlbumArtist;
	public string Title { get; set; } = title;
	public string Date { get; set; } = date;
	public string Genre { get; set; } = genre;

	[Ignore]
	public Artist Artist { get; private set; } = artist;

	public List<Disc> Discs { get; set; } = [];

	public void CleanMultipleArtists(ArtistInfo artistInfo)
	{
		if (AlbumArtistSpecified) return;
		var trackArtists = Discs.SelectMany(d => d.Tracks.Select(t => t.ArtistId)).Distinct().ToList();
		if (trackArtists.Count == 1) return;
		Artist = artistInfo.GetOrCreateArtist("(Multiple Artists)");
	}

	internal Disc GetOrCreateDisc(FieldDictionary fields)
	{
		var discNumberTest = fields.GetShort("Disc #");
		short discNumber = discNumberTest ?? 0;
		var disc = Discs.FirstOrDefault(d => d.DiscNumber == discNumber);
		if (disc == null)
		{
			if (discNumber == 0)
			{
				ErrorLogger.LogError($"Disc # is missing for album: {this}");
			}
			disc = new Disc(this, discNumber);
			Discs.Add(disc);
		}
		return disc;
	}

	public override string ToString()
	{
		return $"{Title} ({Artist.Name})";
	}
}
