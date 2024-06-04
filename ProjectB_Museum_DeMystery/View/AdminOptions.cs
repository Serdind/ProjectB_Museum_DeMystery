public class AdminOptions : View
{
    private static IMuseum museum = Program.Museum;
    public static string Options()
    {
        Console.Clear();
        museum.WriteLine("Overview tours(T)\nAdd tour (A)\nEdit tour (E)\nLog out (L)");
        return ReadLineString();
    }

    public static void ReservationCancelled()
    {
        museum.WriteLine("Reservation cancelled successfully.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static void ReservationCancelDenied()
    {
        museum.WriteLine("Reservation cancellation cancelled.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static void BackOption()
    {
        museum.WriteLine("Insert (Back or B) if you want to go back");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void PressAnything()
    {
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }
    
    public static void Empty()
    {
        museum.WriteLine("You can`t leave it empty.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        Console.Clear();
    }
}