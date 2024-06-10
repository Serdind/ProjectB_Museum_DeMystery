public class AdminOptions : View
{
    
    public static string Options()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Overview tours(T)\nAdd tour (A)\nEdit tour (E)\nLog out (L)");
        museum.WriteLine("");
        return ReadLineString();
    }

    public static void ReservationCancelled()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Reservation cancelled successfully.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void ReservationCancelDenied()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Reservation cancellation cancelled.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void BackOption()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Insert (Back or B) if you want to go back");
        museum.WriteLine("");
    }

    public static void PressAnything()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }
    
    public static void Empty()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("You can`t leave it empty.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static string Confirm()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Are you sure you want to add this tour? (Yes or y) - (No or n)");
        return ReadLineString();
    }

    public static string SelectTours()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Tours from today(T)\nTours after today(A)\nBack(B)");
        return ReadLineString();
    }
}