public class MainMenu : View
{
    private static IMuseum museum = Program.Museum;
    public static string Language()
    {
        museum.WriteLine("Welcome to Het Depot!");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
        museum.WriteLine("Select language");
        museum.WriteLine("English(E)");
        return ReadLineString();
    }

    public static void Intro()
    {
        museum.WriteLine("Here are some tips to help you with the application.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
        museum.WriteLine("When you have to choose between options, you can write the whole option name or you can write the given letter.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
        museum.WriteLine("It doesn't matter if it's lowercase (example: abc) or uppercase (example: ABC).");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
        museum.WriteLine("For example, the options are: Make reservation(R) and Cancel reservation(C)");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
        museum.WriteLine("You can choose to insert the whole option name, for example: Make reservation, or you can choose R.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
        museum.WriteLine("It's recommended to insert the given letter.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
        museum.WriteLine("Press enter when the desired input is given.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }
}
