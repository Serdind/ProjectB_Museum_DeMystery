public class TourInfo : View
{
    private static IMuseum museum = Program.Museum;

    public static void Status()
    {
        museum.WriteLine("Set tour to inactive");
    }

    public static string Date()
    {
        museum.WriteLine("\nWhich date? Example: 01-6-2024 16:00 ");
        return ReadLineString();
    }

    public static void InvalidDate()
    {
        museum.WriteLine("Invalid date format. Please enter a valid date.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static string Language()
    {
        museum.WriteLine("Which language?: ");
        return ReadLineString();
    }

    public static string Time()
    {
        museum.WriteLine("Which time? Example: 16:00:");
        return ReadLineString();
    }

    public static void InvalidTime()
    {
        museum.WriteLine("Invalid time format. Please enter a valid time (Hour:Minutes).");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static string Guide()
    {
        museum.WriteLine("Which guide:");
        return ReadLineString();
    }
    
    public static void NoToursToday()
    {
        museum.WriteLine("No tours available for today.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static string WhichDate()
    {
        museum.WriteLine("Which date of the tour? Example: 01-6-2024:");
        return ReadLineString();
    }

    public static void NoTours()
    {
        museum.WriteLine("No tours available for that day.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static void NoToursTime()
    {
        Console.Clear();
        museum.WriteLine("No tours available for that time.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static void ToursAlreadyExist()
    {
        museum.WriteLine("There already are tours for that time.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }
}