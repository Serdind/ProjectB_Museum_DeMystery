public class NoReservationMade
{
    
    public static void Show()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("No reservation made.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}
