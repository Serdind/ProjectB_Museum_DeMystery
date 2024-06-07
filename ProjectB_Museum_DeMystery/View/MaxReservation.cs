public class MaxReservation : View
{
    
    public static void Show()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("You already made a reservation for today.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void GuideShow()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Visitor already made a reservation for today.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}
