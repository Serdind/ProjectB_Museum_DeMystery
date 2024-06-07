public class CancelReservationConfirmation : View
{
    private static IMuseum museum = Program.Museum;
    public static string Options()
    {
        museum.WriteLine("Are you sure you want to cancel your reservation?\nYes or Y to comfirm\nNo or N to decline");
        return ReadLineString();
    }

    public static void ReservationCancelled()
    {
        
        museum.WriteLine("Reservation cancelled successfully.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void ReservationCancelDenied()
    {
        
        museum.WriteLine("Reservation cancellation cancelled.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}