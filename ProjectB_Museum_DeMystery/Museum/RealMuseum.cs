using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;

public class RealMuseum : IMuseum
{
    public DateTime Now
    {
        get => DateTime.Now;
    }

    public DateTime Today
    {
        get => DateTime.Today;
    }

    public void WriteLine(string line)
    {
        Console.WriteLine(line);
    }

    public string ReadLine()
    {
        return Console.ReadLine();
    }

    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    public void WriteAllText(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public DateTime GetLastWriteTime(string path)
    {
        try
        {
            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.LastWriteTime;
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
        return Console.ReadKey();
    }
}