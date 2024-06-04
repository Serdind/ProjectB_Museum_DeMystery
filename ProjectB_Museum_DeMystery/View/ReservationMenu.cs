public class ReservationMenu : View
{
    private static IMuseum museum = Program.Museum;
    public static string Menu(string qr, Visitor visitor)
    {
        museum.WriteLine("Make reservation(R)");

        if (visitor.ReservationMade(qr))
        {
            museum.WriteLine("My reservations(M)");
            museum.WriteLine("Cancel reservation(C)");
        }
        museum.WriteLine("Help(H)");
        museum.WriteLine("Finish(F)");
        return ReadLineString();
    }

    public static string Help()
    {
        Console.Clear();
        museum.WriteLine("Do you want information about how to use this application first?");
        museum.WriteLine("Yes or Y to comfirm\nNo or N to decline\nBack or b to go back");
        return ReadLineString();
    }

    public static void HelpActive()
    {
        Console.Clear();
        museum.WriteLine("You can read the information at any time using this application with the Help(H) option.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        Console.Clear();
    }

    public static string Finish()
    {
        Console.Clear();
        museum.WriteLine("Are you sure you are done? You haven't made a reservation yet.");
        museum.WriteLine("Yes or Y to comfirm\nNo or N to decline");
        return ReadLineString();
    }
}
