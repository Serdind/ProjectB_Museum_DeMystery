public class TourNotAvailable : View
{
    
    public static void Show()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Tour is not available.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }
}
