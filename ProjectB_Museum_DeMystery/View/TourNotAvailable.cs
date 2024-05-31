public class TourNotAvailable : View
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("Tour is not available.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }
}
