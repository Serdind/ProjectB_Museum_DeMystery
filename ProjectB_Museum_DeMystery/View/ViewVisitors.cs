public class ViewVisitors : View
{
    public static string Show()
    {
        Console.WriteLine("View visitors(V)\nStart tour(S)");
        return ReadLineString();
    }
}