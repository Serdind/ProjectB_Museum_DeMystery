
public class ReservationMenu : View
{
    public static string Menu(string qr, Visitor visitor)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Make reservation(R)");

        if (visitor.ReservationMade(qr))
        {
            museum.WriteLine("My reservations(M)");
            museum.WriteLine("Cancel reservation(C)");
            museum.WriteLine("Rebook reservation(B)"); // Nieuwe optie voor herboeken
        }
        museum.WriteLine("Help(H)");
        museum.WriteLine("Finish(F)");
        return ReadLineString();
    }

    public static string Help()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Do you want any instructions about how to use this application first?");
        museum.WriteLine("Yes or Y to confirm\nNo or N to decline\nBack or b to go back");
        return ReadLineString();
    }

    public static void HelpActive()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("You can read the information at any time using this application with the Help(H) option.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static string Finish()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Are you sure you are done? You haven't made a reservation yet.");
        museum.WriteLine("Yes or Y to confirm\nNo or N to decline");
        return ReadLineString();
    }
    
}
