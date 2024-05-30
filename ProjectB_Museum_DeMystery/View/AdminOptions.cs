public class AdminOptions : View
{
    private static IMuseum museum = Program.Museum;
    public static string Options()
    {
        museum.WriteLine("Overview tours(T)\nAdd tour (A)\nEdit tour (E)\nLog out (L)");
        return ReadLineString();
    }

    public static void ReservationCancelled()
    {
        museum.WriteLine("Reservation cancelled successfully.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static void ReservationCancelDenied()
    {
        museum.WriteLine("Reservation cancellation cancelled.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static void BackOption()
    {
        museum.WriteLine("Insert (Back or B) if you want to go back");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }
}