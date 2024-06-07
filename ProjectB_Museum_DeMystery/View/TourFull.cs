public class TourFull : View
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("Tour is full.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}
