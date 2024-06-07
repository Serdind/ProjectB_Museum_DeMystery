public class TourInfo : View
{
    

    public static void Status()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Set tour to inactive");
    }

    public static string Date()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("\nWhich date do you want to set it to? Example: 01-6-2024 16:00 ");
        return ReadLineString();
    }

    public static void InvalidDate()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Invalid date format. Please enter a valid date.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static string Language()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Which language do you want to set it to?: ");
        return ReadLineString();
    }

    public static string Time()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Which time do you want to set it to? Example: 16:00:");
        return ReadLineString();
    }

    public static void InvalidTime()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Invalid time format. Please enter a valid time (Hour:Minutes).");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static string Guide()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Which guide:");
        return ReadLineString();
    }
    
    public static void NoToursToday()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("No tours available for today.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static string WhichDate()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Which date of the tour do you want to edit? Example: 01-6-2024:");
        return ReadLineString();
    }

    public static void NoTours()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("No tours available for that day.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void NoToursTime()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("No tours available for that time.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void ToursAlreadyExist()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("There already are tours for that time.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}