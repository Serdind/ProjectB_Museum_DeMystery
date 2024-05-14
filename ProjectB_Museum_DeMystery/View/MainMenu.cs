public class MainMenu : View
{
    public static string Menu()
    {
        Console.WriteLine("Welcome to Het Depot!");
        Console.WriteLine("Select language");
        Console.WriteLine("English(E)");
        return ReadLineString();
    }
}