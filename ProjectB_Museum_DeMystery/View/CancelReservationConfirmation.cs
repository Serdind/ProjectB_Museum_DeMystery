public class CancelReservationConfirmation : View
{
    public static string Options()
    {
        Console.WriteLine("Are you sure you want to cancel your reservation?\nYes or Y to comfirm\nNo or N to decline");
        return ReadLineString();
    }

    public static void ReservationCancelled()
    {
        Console.WriteLine("Reservation cancelled successfully.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void ReservationCancelDenied()
    {
        Console.WriteLine("Reservation cancellation cancelled.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }
}