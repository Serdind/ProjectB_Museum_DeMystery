public class TourNotFound
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("Tour not found.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
    }
}
