namespace MediaXML;

internal static class ErrorLogger
{
	public static int ErrorCount { get; private set; } = 0;

	public static void LogError(string message)
	{
		ErrorCount++;
		Console.WriteLine($"{ErrorCount}: {message}");
	}
}
