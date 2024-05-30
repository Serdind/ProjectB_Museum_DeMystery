public class TourInfo : View
{
    private static IMuseum museum = Program.Museum;
    public static string Name()
    {
        museum.WriteLine("Name:");
        return ReadLineString();
    }

    public static void Status()
    {
        museum.WriteLine("Set tour to inactive");
    }

    public static string Date()
    {
        museum.WriteLine("\nDate (Day-Month-Year Hour:Minutes): ");
        return ReadLineString();
    }

    public static void InvalidDate()
    {
        museum.WriteLine("Invalid date format. Please enter a valid date.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static string Language()
    {
        museum.WriteLine("\nLanguage: ");
        return ReadLineString();
    }

    public static string Time()
    {
        museum.WriteLine("Time (Hour:Minutes):");
        return ReadLineString();
    }

    public static void InvalidTime()
    {
        museum.WriteLine("Invalid time format. Please enter a valid time (Hour:Minutes).");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static string Guide()
    {
        museum.WriteLine("Guide:");
        return ReadLineString();
    }

    public static void TourRemoved()
    {
        museum.WriteLine("Tour removed successfully.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static void TourRestored()
    {
        museum.WriteLine("Tour successfully restored.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static void NoToursToday()
    {
        museum.WriteLine("No tours available for today.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static string WhichDate()
    {
        museum.WriteLine("Which date? (Day-Month-Year)");
        return ReadLineString();
    }

    public static void NoTours()
    {
        museum.WriteLine("No tours available for that day.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }
}