public class TourEmpty : View
{
    
    public static void Show()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Tour is empty.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void NoVisitorsInTour()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("No visitors found for the specified tour.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}
