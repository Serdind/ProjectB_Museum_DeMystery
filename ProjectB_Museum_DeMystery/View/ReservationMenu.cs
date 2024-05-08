public class ReservationMenu : View
{
    public static string Menu(int tour)
    {
        Console.WriteLine("Make reservation (E)");

        if (tour != 0)
        {
            Console.WriteLine("My reservations (M)");
            Console.WriteLine("Cancel reservation (C)");
        }
        return ReadLineString();
    }
}