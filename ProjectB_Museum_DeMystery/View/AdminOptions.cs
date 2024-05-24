public class AdminOptions : View
{
    public static string Options()
    {
        Console.WriteLine("Overview tours(T)\nAdd tour (A)\nEdit tour (E)\nLog out (L)");
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

    public static void BackOption()
    {
        Console.WriteLine("Insert (Back or B) if you want to go back");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }
}