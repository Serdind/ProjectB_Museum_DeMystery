public class TourFull : View
{
    
    public static void Show()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Tour is full.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }
}
