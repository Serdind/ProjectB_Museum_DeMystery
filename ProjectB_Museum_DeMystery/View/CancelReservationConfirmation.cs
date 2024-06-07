public class CancelReservationConfirmation : View
{
    
    public static string Options()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Are you sure you want to cancel your reservation?\nYes or Y to comfirm\nNo or N to decline");
        return ReadLineString();
    }

    public static void ReservationCancelled()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Reservation cancelled successfully.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void ReservationCancelDenied()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Reservation cancellation cancelled.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}