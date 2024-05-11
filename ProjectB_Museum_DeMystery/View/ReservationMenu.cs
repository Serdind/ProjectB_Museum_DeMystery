public class ReservationMenu : View
{
    public static string Menu(string qr)
    {
        Console.WriteLine("Make reservation (E)");

        if (Visitor.ReservationMade(qr))
        {
            Console.WriteLine("My reservations (M)");
            Console.WriteLine("Cancel reservation (C)");
        }
        return ReadLineString();
    }
}