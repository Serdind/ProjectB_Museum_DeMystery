using System.Text.Json;

public class Model<T>
{
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