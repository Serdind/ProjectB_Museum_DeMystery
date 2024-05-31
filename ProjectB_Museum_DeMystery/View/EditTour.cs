public class EditTour : View
{
    private static IMuseum museum = Program.Museum;
    public static string EditOptions()
    {
        museum.WriteLine("What do you want to change? Time(T) Language(L) Guide(G) Status(S) Go back(B)");
        return ReadLineString();
    }

    public static void NameSet(string name)
    {
        museum.WriteLine($"Name set to {name}");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void DateSet(DateTime date)
    {
        museum.WriteLine($"Date set to {date}");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void TimeSet(TimeSpan date)
    {
        museum.WriteLine($"Time set to {date}");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void LanguageSet(string language)
    {
        museum.WriteLine($"Language set to {language}");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void GuideSet(string guide)
    {
        museum.WriteLine($"Guide set to {guide}");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }
    public static void StatusSet(bool status)
    {
        if (status == true)
        {
            museum.WriteLine($"Status set to active");
            museum.WriteLine("Press anything to continue...\n");
            museum.ReadKey();
        }
        else
        {
            museum.WriteLine($"Status set to inactive");
            museum.WriteLine("Press anything to continue...\n");
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