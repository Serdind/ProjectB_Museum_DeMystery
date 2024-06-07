public class ViewVisitors : View
{
    
    public static string Show()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("View visitors(V)\nStart tour(S)\nLog out(L)");
        return ReadLineString();
    }
}
