public class AdminOptions : View
{
    public static string Options()
    {
        Console.WriteLine("Overview tours(T)\nAdd tour (A)\nEdit tour (E)");
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