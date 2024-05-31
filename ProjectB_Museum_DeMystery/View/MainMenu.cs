public class MainMenu : View
{
    private static IMuseum museum = Program.Museum;
    public static void Welcome()
    {
        museum.WriteLine("Welcome to The Depot!");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void Intro()
    {
        museum.WriteLine("Here are some tips to help you with the application.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
        museum.WriteLine("When you have to choose between options, you can write the whole option name or you can write the given letter.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
        museum.WriteLine("It doesn't matter if it's lowercase (example: abc) or uppercase (example: ABC).");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
        museum.WriteLine("For example, the options are: Make reservation(R) and Cancel reservation(C)");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
        museum.WriteLine("You can choose to insert the whole option name, for example: Make reservation, or you can choose R.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
        museum.WriteLine("It's recommended to insert the given letter.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
        museum.WriteLine("Press enter when the desired input is given.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }
}
