public class GuideOptions : View
{
    public static string Options()
    {
        Console.WriteLine("Add visitor(A)\nRemove visitor(R)");
        return ReadLineString();
    }

    public static string ViewTours()
    {
        Console.WriteLine("My tours(M)");
        return ReadLineString();
    }
}