using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;

public interface IMuseum
{
    DateTime Now { get; }

    void WriteLine(string line);

    string ReadLine();

    string ReadAllText(string path);

    void WriteAllText(string path, string contents);

    bool FileExists(string path);

    DateTime GetLastWriteTime(string path);

    bool TryParseExact(string? s, string? format, IFormatProvider? provider, DateTimeStyles style, out DateTime result);

    DateTime MinValue { get; }

    ConsoleKeyInfo ReadKey();
}