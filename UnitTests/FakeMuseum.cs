using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

public class FakeMuseum : IMuseum
{
    private DateTime? _now = DateTime.Now;
    private StringBuilder _renderedOutput = new StringBuilder();

    private DateTime? _today = DateTime.Now;

    public DateTime Now
    {
        get => _now ?? throw new NullReferenceException();
        set => _now = value;
    }

    public DateTime Today
    {
        get => _today ?? throw new NullReferenceException();
        set => _today = value;
    }

    public List<string> LinesWritten { get; } = new();

    public void WriteLine(string line)
    {
        LinesWritten.Add(line);
    }

    public List<string> LinesToRead { get; set; } = new();

    public string ReadLine()
    {
        string firstLine = LinesToRead.ElementAt(0);
        LinesToRead.RemoveAt(0);
        return firstLine;
    }

    public Dictionary<string, string> Files = new();
    
    public void ClearFiles()
    {
        Files.Clear();
    }

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

    public DateTime GetLastWriteTime(string path)
    {
        return DateTime.Now;
    }

    public bool TryParseExact(string? s, string? format, IFormatProvider? provider, DateTimeStyles style, out DateTime result)
    {
        return DateTime.TryParseExact(s, format, provider, style, out result);
    }

    public DateTime MinValue
    {
        get => DateTime.MinValue;
    }

    public ConsoleKeyInfo ReadKey()
    {
        return new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false);
    }

    public void Render(Table table)
    {
        var originalConsoleOut = Console.Out;
        var writer = new StringWriter();
        Console.SetOut(writer);

        AnsiConsole.Render(table);

        Console.SetOut(originalConsoleOut);
        _renderedOutput.Append(writer.ToString());
    }

    public string GetRenderedOutput()
    {
        return _renderedOutput.ToString();
    }
}