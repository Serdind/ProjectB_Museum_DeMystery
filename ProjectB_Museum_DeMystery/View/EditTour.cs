public class EditTour : View
{
    public static string EditOptions()
    {
        Console.WriteLine("What do you want to change? Name(N) Date(D) Time(T) Language(L) Guide(G) Status(S) Go back(B)");
        return ReadLineString();
    }

    public static void NameSet(string name)
    {
        Console.WriteLine($"Name set to {name}");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void DateSet(DateTime date)
    {
        Console.WriteLine($"Date set to {date}");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void TimeSet(TimeSpan date)
    {
        Console.WriteLine($"Time set to {date}");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void LanguageSet(string language)
    {
        Console.WriteLine($"Language set to {language}");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void GuideSet(string guide)
    {
        Console.WriteLine($"Guide set to {guide}");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }
    public static void StatusSet(bool status)
    {
        if (status == true)
        {
            Console.WriteLine($"Status set to active");
            Console.WriteLine("Press any key to continue...\n");
            Console.ReadKey(true);
        }
        else
        {
            Console.WriteLine($"Status set to inactive");
            Console.WriteLine("Press any key to continue...\n");
            Console.ReadKey(true);
        }
    }
}