public class TourNotAvailable : View
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("");
        museum.WriteLine("Tour is not available.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }
}
