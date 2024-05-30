public class LoginMenu : View
{
    private static IMuseum museum = Program.Museum;
    public static string Login()
    {
        museum.WriteLine("Login(L)");
        return ReadLineString();
    }
}