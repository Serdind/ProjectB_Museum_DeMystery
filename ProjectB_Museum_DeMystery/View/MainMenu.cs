public class MainMenu : View
{
    public static string Menu()
    {
        Console.WriteLine("Welcome to Het Depot!");
        Console.WriteLine("select language  /   selecteer taal");
        Console.WriteLine("English(E)   /   Nederlands(N)");
        return ReadLineString();
    }
}