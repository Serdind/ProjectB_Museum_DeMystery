public class ViewVisitors : View
{
    private static IMuseum museum = Program.Museum;
    public static string Show()
    {
        museum.WriteLine("View visitors(V)\nStart tour(S)\nLog out(L)");
        return ReadLineString();
    }
}
