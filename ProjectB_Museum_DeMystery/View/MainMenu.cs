public class MainMenu : View
{
    
    public static void Welcome()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Welcome to The Depot!");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void Intro()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Here are some tips to help you with the application.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        museum.WriteLine("When you have to choose between options, you can write the whole option name or you can write the given letter.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        museum.WriteLine("It doesn't matter if it's lowercase (example: abc) or uppercase (example: ABC).");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        museum.WriteLine("For example, the options are: Make reservation(R) and Cancel reservation(C)");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        museum.WriteLine("You can choose to insert the whole option name, for example: Make reservation, or you can choose R.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        museum.WriteLine("It's recommended to insert the given letter.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        museum.WriteLine("Press enter when the desired input is given.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void Goodbye()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Thank you for visiting The Depot!");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}
