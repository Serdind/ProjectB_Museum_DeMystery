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
        museum.WriteLine("Log out(L)");
        return ReadLineString();
    }
}
