public class EditTour : View
{
    private static IMuseum museum = Program.Museum;
    public static string EditOptions()
    {
        museum.WriteLine("What do you want to change? Time(T) Language(L) Status(S) Go back(B)");
        return ReadLineString();
    }

    public static void TimeSet(TimeSpan time)
    {
        
        museum.WriteLine($"Time set to {time:hh\\:mm}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void LanguageSet(string language)
    {
        
        museum.WriteLine($"Language set to {language}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void GuideSet(string guide)
    {
        
        museum.WriteLine($"Guide set to {guide}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }
    public static void StatusSet(bool status)
    {
        if (status == true)
        {
            
            museum.WriteLine($"Status set to active");
            museum.WriteLine("Press anything to continue...");
            museum.ReadKey();
            
        }
        else
        {
            
            museum.WriteLine($"Status set to inactive");
            museum.WriteLine("Press anything to continue...");
            museum.ReadKey();
            
        }
    }

    public static string TimeEdit()
    {
        museum.WriteLine("Enter the time from the tours you want to edit (HH:mm):");
        return ReadLineString();
    }

    public static string NewTime()
    {
        museum.WriteLine("Enter a new time (HH:mm):");
        return ReadLineString();
    }
}