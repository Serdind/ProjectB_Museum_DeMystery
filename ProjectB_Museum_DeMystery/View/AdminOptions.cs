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
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void ReservationCancelDenied()
    {
        museum.WriteLine("Reservation cancellation cancelled.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void BackOption()
    {
        museum.WriteLine("Insert (Back or B) if you want to go back");
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
        
    }

    public static string Confirm()
    {
        
        museum.WriteLine("Are you sure you want to add this tour? (Yes or y) - (No or n)");
        return ReadLineString();
    }

    public static string SelectTours()
    {
        
        museum.WriteLine("Tours from today(T)\nTours after today(A)\nBack(B)");
        return ReadLineString();
    }
}