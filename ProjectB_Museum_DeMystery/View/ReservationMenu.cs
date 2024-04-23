public class ReservationMenu : View
{
    public static string Menu()
    {
        Console.WriteLine("Make reservation(E)\nMy reservations(M)\nCancel reservation(C)");
        return ReadLineString();
    }
}