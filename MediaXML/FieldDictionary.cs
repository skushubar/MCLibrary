namespace MediaXML;

internal class FieldDictionary(Dictionary<string, string> dict) : Dictionary<string, string>(dict)
{

	public string GetString(string key)
	{
		return TryGetValue(key, out var value) ? value : string.Empty;
	}

	public string GetStringNotEmpty(string key)
	{
		var t = GetString(key);
		if (string.IsNullOrEmpty(t))
		{
			ErrorLogger.LogError($"Missing: {key}.");
		}
		return t;
	}

	internal short? GetShort(string key)
	{
		if (!TryGetValue(key, out var strvalue)) return null;
		if (!short.TryParse(strvalue, out var value)) return null;
		return value;
	}

	internal short GetShortNotEmpty(string key)
	{
		if (!TryGetValue(key, out var strvalue))
		{
			ErrorLogger.LogError($"Missing : {key}.");
			return -1;
		}
		if (!short.TryParse(strvalue, out var value))
		{
			ErrorLogger.LogError($"Invalid value for {key}: {strvalue}.");
			return -1;
		}
		return value;
	}
}
