public class ReservationMenu : View
{
    public static string Menu(string qr, Visitor visitor)
    {
        Console.WriteLine("Make reservation (E)");

        if (visitor.ReservationMade(qr))
        {
            Console.WriteLine("My reservations (M)");
            Console.WriteLine("Cancel reservation (C)");
        }
        Console.WriteLine("Done (D)");
        return ReadLineString();
    }
}
