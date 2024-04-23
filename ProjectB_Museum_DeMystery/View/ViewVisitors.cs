public class ViewVisitors : View
{
    public static string Show()
    {
        Console.WriteLine("View visitors(V)");
        return ReadLineString();
    }
}