using System.Text.Json;

public class Model<T>
{
    protected DateTime Now { get => Program.Museum.Now; }
    private static readonly string FileNameTours = GetFileNameTours();
    private static readonly string FileNameAdmins = GetFileNameAdmins();
    private static readonly string FileNameGuides = GetFileNameGuides();
    private static readonly string FileNameVisitors = GetFileNameVisitors();
    private static readonly string FileNameUniqueCodes = GetFileNameUniqueCodes();
    protected static readonly List<T> _items_tours = ReadAllTours();
    protected static readonly List<T> _items_admins = ReadAllAdmins();
    protected static readonly List<T> _items_guides = ReadAllGuides();
    protected static readonly List<T> _items_visitors = ReadAllVisitors();
    protected static readonly List<T> _items_unique_codes = ReadAllUniqueCodes();

    public static List<T> ReadAllTours()
    {
        string json = Program.Museum.ReadAllText(FileNameTours);

        if (string.IsNullOrEmpty(json))
        {
            return new List<T>();
        }
        else
        {
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }

    public static List<T> ReadAllAdmins()
    {
        string json = Program.Museum.ReadAllText(FileNameAdmins);

        if (string.IsNullOrEmpty(json))
        {
            return new List<T>();
        }
        else
        {
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }

    public static List<T> ReadAllGuides()
    {
        string json = Program.Museum.ReadAllText(FileNameGuides);

        if (string.IsNullOrEmpty(json))
        {
            return new List<T>();
        }
        else
        {
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }

    public static List<T> ReadAllVisitors()
    {
        string json = Program.Museum.ReadAllText(FileNameVisitors);

        if (string.IsNullOrEmpty(json))
        {
            return new List<T>();
        }
        else
        {
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }

    public static List<T> ReadAllUniqueCodes()
    {
        string json = Program.Museum.ReadAllText(FileNameUniqueCodes);

        if (string.IsNullOrEmpty(json))
        {
            return new List<T>();
        }
        else
        {
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }

    public static void WriteAllTours()
    {
        JsonSerializerOptions options = new() { WriteIndented = true };
        string json = JsonSerializer.Serialize(_items_tours, options);
        Program.Museum.WriteAllText(FileNameTours, json);
    }

    public static void WriteAllAdmins()
    {
        JsonSerializerOptions options = new() { WriteIndented = true };
        string json = JsonSerializer.Serialize(_items_admins, options);
        Program.Museum.WriteAllText(FileNameAdmins, json);
    }

    public static void WriteAllGuides()
    {
        JsonSerializerOptions options = new() { WriteIndented = true };
        string json = JsonSerializer.Serialize(_items_guides, options);
        Program.Museum.WriteAllText(FileNameGuides, json);
    }

    public static void WriteAllVisitors()
    {
        JsonSerializerOptions options = new() { WriteIndented = true };
        string json = JsonSerializer.Serialize(_items_visitors, options);
        Program.Museum.WriteAllText(FileNameVisitors, json);
    }

    public static void WriteAllUniqueCodes()
    {
        JsonSerializerOptions options = new() { WriteIndented = true };
        string json = JsonSerializer.Serialize(_items_unique_codes, options);
        Program.Museum.WriteAllText(FileNameUniqueCodes, json);
    }

    public static string GetFileNameTours()
    {
        return $"Data/tours.json";
    }

    public static string GetFileNameAdmins()
    {
        return $"Data/admins.json";
    }

    public static string GetFileNameGuides()
    {
        return $"Data/guides.json";
    }

    public static string GetFileNameVisitors()
    {
        return $"Data/visitors.json";
    }

    public static string GetFileNameUniqueCodes()
    {
        return $"Data/unique_codes.json";
    }
}