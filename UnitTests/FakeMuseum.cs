public class FakeMuseum : IMuseum
{
    private DateTime? _now = null;

    public DateTime Now
    {
        get => _now ?? throw new NullReferenceException();
        set => _now = value;
    }

    public List<string> LinesWritten { get; } = new();

    public void WriteLine(string line)
    {
        LinesWritten.Add(line);
    }
    
    public List<string> LinesToRead { private get; set; } = new();

    public string ReadLine()
    {
        string firstLine = LinesToRead.ElementAt(0);
        LinesToRead.RemoveAt(0);
        return firstLine;
    }

    public Dictionary<string, string> Files = new();

    public string ReadAllText(string path)
    {
        return Files[path];
    }

    public void WriteAllText(string path, string contents)
    {
        Files[path] = contents;
    }

    public string GetWrittenLinesAsString()
    {
        return string.Join("\n", LinesWritten);
    }

     public bool FileExists(string path)
    {
        return Files.ContainsKey(path);
    }
}