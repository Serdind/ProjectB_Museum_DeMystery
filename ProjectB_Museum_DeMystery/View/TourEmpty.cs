public class TourEmpty : View
{
    
    public static void Show()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Tour is empty.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void NoVisitorsInTour()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("No visitors found for the specified tour.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }
}
