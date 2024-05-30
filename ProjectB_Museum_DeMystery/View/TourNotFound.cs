public class TourNotFound
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("Tour not found.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static void RemovedToursNotFound()
    {
        museum.WriteLine("No tour found in the removedTours.json file.");
    }
}
