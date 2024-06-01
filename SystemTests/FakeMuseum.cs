using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;

public class FakeMuseum : IMuseum
{
    private DateTime? _now = DateTime.Now;
    private DateTime? _today = DateTime.Today;
    private int _readKeyCount = 0;

    private int _linesRead = 0;
    private readonly Dictionary<string, int> _filesTimesRead = new();
    private readonly Dictionary<string, List<string>> _previousFiles = new();

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
    public List<string> LinesToRead { private get; set; } = new();
    public Dictionary<string, string> Files = new();
    public bool IncludeLinesReadInLinesWritten { get; set; } = false;

    

    public void WriteLine(string line)
    {
        LinesWritten.Add(line);
    }
    
    public string ReadLine()
    {
        if (_linesRead < LinesToRead.Count)
        {
            string line = LinesToRead.ElementAt(_linesRead++);
            if (IncludeLinesReadInLinesWritten)
                WriteLine(line);
            return line;
        }
        else
        {
            throw new ArgumentOutOfRangeException($"Index out of range: _linesRead = {_linesRead}, LinesToRead.Count = {LinesToRead.Count}");
        }
    }

    public string ReadAllText(string path)
    {
        _filesTimesRead[path] = _filesTimesRead.GetValueOrDefault(path, 0) + 1;
        return Files[path];
    }

    public void WriteAllText(string path, string content)
    {
        if (!_previousFiles.ContainsKey(path))
            _previousFiles[path] = new();
        _previousFiles[path].Add(Files[path]);
        Files[path] = content;
    }

    private List<string> DebugInfoNow()
    {
        return new() {
            "--- Now",
            _now?.ToString("O") ?? "null"
        };
    }

    private List<string> DebugInfoLinesToRead()
    {
        return new() {
            $"--- LinesToRead",
            $"--- {LinesToRead.Count} lines",
            $"--- {(_linesRead == LinesToRead.Count ? "all" : "only")} {_linesRead} read",
            string.Join("\n", LinesToRead)
        };
    }

    private List<string> DebugInfoLinesWritten()
    {
        return new() {
            $"--- LinesWritten",
            $"--- {LinesWritten.Count} lines",
            $"--- {(IncludeLinesReadInLinesWritten ? "including" : "excluding")} lines read",
            string.Join("\n", LinesWritten)
        };
    }

    private List<string> DebugInfoFiles()
    {
        List<string> info = new() { $"--- Files ({Files.Count} files)" };
        foreach ((string path, string currentContent) in Files)
        {
            List<string> previousContents = _previousFiles.GetValueOrDefault(path, new());
            info.Add($"--- Files[{path}]");
            info.Add($"--- {_filesTimesRead.GetValueOrDefault(path, 0)} times read");
            info.Add($"--- {previousContents.Count} times written");
            info.Add($"--- {previousContents.Count + 1} versions");
            IEnumerable<(string, int)> indexedVersions = previousContents.Select((item, index) => (item, index));
            foreach ((string previousContent, int version) in indexedVersions)
            {
                info.Add($"--- version {version}");
                info.Add(previousContent);
            }
            if (previousContents.Count > 0)
                info.Add($"--- version {previousContents.Count}");
            info.Add(currentContent);
        }
        return info;
    }

    public override string ToString()
    {
        List<string> info = new();
        info.AddRange(DebugInfoNow());
        info.AddRange(DebugInfoLinesToRead());
        info.AddRange(DebugInfoLinesWritten());
        info.AddRange(DebugInfoFiles());
        info.Add("---");
        return string.Join("\n", info);
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
        _readKeyCount++;
        return new ConsoleKeyInfo('\0', ConsoleKey.Enter, false, false, false);
    }
}