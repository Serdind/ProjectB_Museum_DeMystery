public class MainMenu : View
{
    public static string Language()
    {
        Console.WriteLine("Welcome to Het Depot!");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("Select language");
        Console.WriteLine("English(E)");
        return ReadLineString();
    }

    public static void Intro()
    {
        Console.WriteLine("Here are some tips to help you with the application.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("When you have to choose between options, you can write the whole option name or you can write the given letter.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("It doesn't matter if it's lowercase (example: abc) or uppercase (example: ABC).");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("For example, the options are: Make reservation(R) and Cancel reservation(C)");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("You can choose to insert the whole option name, for example: Make reservation, or you can choose R.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("It's recommended to insert the given letter.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
        Console.WriteLine("Press enter when the desired input is given.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }
}
