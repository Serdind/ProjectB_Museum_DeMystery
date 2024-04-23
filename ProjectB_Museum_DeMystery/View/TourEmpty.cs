public class TourEmpty : View
{
    public static void Show()
    {
        Console.WriteLine("Tour is empty.");
    }

    public static void RemovedToursEmpty()
    {
        Console.WriteLine("The removedTours.json file is empty.");
    }

    public static void NoVisitorsInTour()
    {
        Console.WriteLine("No visitors found for the specified tour.");
    }
}