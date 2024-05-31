public class TourEmpty : View
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("Tour is empty.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void NoVisitorsInTour()
    {
        museum.WriteLine("No visitors found for the specified tour.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }
}
