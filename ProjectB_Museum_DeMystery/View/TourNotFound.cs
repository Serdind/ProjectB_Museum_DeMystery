public class TourNotFound
{
    public static void Show()
    {
        Console.WriteLine("Tour not found.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void RemovedToursNotFound()
    {
        Console.WriteLine("No tour found in the removedTours.json file.");
    }
}
