public class CancelReservationConfirmation : View
{
    public static string Options()
    {
        Console.WriteLine("Are you sure you want to cancel your reservation? (Y/N)");
        return ReadLineString();
    }

    public static void ReservationCancelled()
    {
        Console.WriteLine("Reservation cancelled successfully.");
    }
    
    public static void ReservationCancelDenied()
    {
        Console.WriteLine("Reservation cancellation cancelled.");
    }
}