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
        Console.WriteLine("\nDate (Day-Month-Year Hour:Minutes): ");
        Console.WriteLine("\nDate (Day-Month-Year Hour:Minutes): ");
        return ReadLineString();
    }

    public static void InvalidDate()
    {
        Console.WriteLine("Invalid date format. Please enter a valid date.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static string Language()
    {
        Console.WriteLine("\nLanguage: ");
        return ReadLineString();
    }

    public static string Time()
    {
        Console.WriteLine("Time (Hour:Minutes):");
        Console.WriteLine("Time (Hour:Minutes):");
        return ReadLineString();
    }

    public static void InvalidTime()
    {
        Console.WriteLine("Invalid time format. Please enter a valid time (Hour:Minutes).");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("Invalid time format. Please enter a valid time (Hour:Minutes).");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static string Guide()
    {
        Console.WriteLine("Guide:");
        return ReadLineString();
    }

    public static void TourRemoved()
    {
        Console.WriteLine("Tour removed successfully.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void TourRestored()
    {
        Console.WriteLine("Tour successfully restored.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void NoToursToday()
    {
        Console.WriteLine("No tours available for today.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static string WhichDate()
    {
        Console.WriteLine("Which date? (Day-Month-Year)");
        return ReadLineString();
    }
}
