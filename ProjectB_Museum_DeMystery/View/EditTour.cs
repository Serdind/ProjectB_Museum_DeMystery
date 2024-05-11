public class EditTour : View
{
    public static string EditOptions()
    {
        Console.WriteLine("What do you want to change? Name(N) Date(D) Time(T) Language(L) Guide(G)");
        return ReadLineString();
    }

    public static void NameSet(string name)
    {
        Console.WriteLine($"Name set to {name}");
    }

    public static void DateSet(DateTime date)
    {
        Console.WriteLine($"Date set to {date}");
    }

    public static void TimeSet(TimeSpan date)
    {
        Console.WriteLine($"Time set to {date}");
    }

    public static void LanguageSet(string language)
    {
        Console.WriteLine($"Language set to {language}");
    }

    public static void GuideSet(string guide)
    {
        Console.WriteLine($"Guide set to {guide}");
    }
    public static void StatusSet(bool status)
    {
        if (status == true)
        {
            Console.WriteLine($"Status set to active");
        }
        else
        {
            Console.WriteLine($"Status set to inactive");
        }
    }
}