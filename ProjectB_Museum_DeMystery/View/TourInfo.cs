public class TourInfo : View
{
    public static string Name()
    {
        Console.WriteLine("Name:");
        return ReadLineString();
    }

    public static void Status()
    {
        Console.WriteLine("Set tour to inactive");
    }

    public static string Date()
    {
        Console.WriteLine("\nDate (Y-M-D H:M:S): ");
        return ReadLineString();
    }

    public static void InvalidDate()
    {
        Console.WriteLine("Invalid date format. Please enter a valid date.");
    }

    public static string Language()
    {
        Console.WriteLine("\nLanguage: ");
        return ReadLineString();
    }

    public static string Time()
    {
        Console.WriteLine("Time (H:M:S):");
        return ReadLineString();
    }

    public static void InvalidTime()
    {
        Console.WriteLine("Invalid time format. Please enter a valid time (H:M:S).");
    }

    public static string Guide()
    {
        Console.WriteLine("Guide:");
        return ReadLineString();
    }

    public static void TourRemoved()
    {
        Console.WriteLine("Tour removed successfully.");
    }

    public static void TourRestored()
    {
        Console.WriteLine("Tour successfully restored.");
    }
}