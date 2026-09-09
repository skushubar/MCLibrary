using CsvHelper;
using MediaXML;
using System.Globalization;
using System.Xml.Serialization;

var filePath = @"C:\Temp\MC Library.xml";

var mpl = ReadMpl(filePath);

var builder = new Builder(mpl);
if (!builder.Build())
{
	ErrorLogger.LogError("Failed to build the album collection.");
	throw new InvalidOperationException("Failed to build the album collection.");
}

Console.WriteLine($"Albums: {builder.Albums.Count}");
Console.WriteLine($"Artists: {builder.Artists.Count}");

WriteCsvs(builder);

void WriteCsvs(Builder builder)
{
	using (var writer = new StreamWriter(@"C:\Temp\Albums.csv"))
	using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
	{
		csv.WriteRecords(builder.Albums);
	}

	using (var writer = new StreamWriter(@"C:\Temp\Artists.csv"))
	using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
	{
		csv.WriteRecords(builder.Artists);
	}

	using (var writer = new StreamWriter(@"C:\Temp\Discs.csv"))
	using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
	{
		var disc = builder.Albums.SelectMany(a => a.Discs).ToList();
		Console.WriteLine($"Discs: {disc.Count}");
		csv.WriteRecords(disc);
	}

	using (var writer = new StreamWriter(@"C:\Temp\Tracks.csv"))
	using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
	{
		var tracks = builder.Albums.SelectMany(a => a.Discs).SelectMany(d => d.Tracks).ToList();
		Console.WriteLine($"Tracks: {tracks.Count}");
		csv.WriteRecords(tracks);
	}
}

//var tracks = new List<Track>();
//foreach (var item in mpl.Items)
//{
//	var track = new Track();
//	FieldDictionary fields = new(item.Fields.ToDictionary(f => f.Name, f => f.Value));
//	track.MediaType = fields.GetStringNotEmpty("Media Type");
//	if (track.MediaType == "Video") continue;
//	track.Filename = fields.GetStringNotEmpty("Filename");
//	track.Artist = fields.GetStringNotEmpty("Artist");
//	track.Album = fields.GetStringNotEmpty("Album");
//	track.Name = fields.GetStringNotEmpty("Name");
//	track.Genre = fields.GetString("Genre");
//	track.Rating = fields.GetShort("Rating");
//	track.TrackNumber = fields.GetShort("Track #");
//	track.DiscNumber = fields.GetShort("Disc #");
//	track.TotalTracks = fields.GetShort("Total Tracks");
//	tracks.Add(track);
//}

//using (var writer = new StreamWriter(@"C:\Temp\MC Library.csv"))
//using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
//{
//	csv.WriteRecords(tracks);
//}

//Console.WriteLine(mpl);

static MPL ReadMpl(string filePath)
{
	MPL? mpl;
	using (var reader = new StreamReader(filePath))
	{
		mpl = (MPL?)new XmlSerializer(typeof(MPL)).Deserialize(reader);
	}
	if (mpl is null)
	{
		Console.WriteLine("Failed to deserialize the XML file.");
		throw new InvalidOperationException("Failed to deserialize the XML file.");
	}

	return mpl;
}