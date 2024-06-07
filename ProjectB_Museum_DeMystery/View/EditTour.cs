public class EditTour : View
{
    
    public static string EditOptions()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("What do you want to change? Time(T) Language(L) Status(S) Go back(B)");
        return ReadLineString();
    }

    public static void TimeSet(TimeSpan time)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine($"Time set to {time:hh\\:mm}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void LanguageSet(string language)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine($"Language set to {language}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void GuideSet(string guide)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine($"Guide set to {guide}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }
    public static void StatusSet(bool status)
    {
        if (status == true)
        {
            IMuseum museum = Program.Museum;
            museum.WriteLine($"Status set to active");
            museum.WriteLine("Press anything to continue...");
            museum.ReadKey();
            
        }
        else
        {
            IMuseum museum = Program.Museum;
            museum.WriteLine($"Status set to inactive");
            museum.WriteLine("Press anything to continue...");
            museum.ReadKey();
            
        }
    }

    public static string TimeEdit()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Enter the time from the tours you want to edit (HH:mm):");
        return ReadLineString();
    }

    public static string NewTime()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Enter a new time (HH:mm):");
        return ReadLineString();
    }
}