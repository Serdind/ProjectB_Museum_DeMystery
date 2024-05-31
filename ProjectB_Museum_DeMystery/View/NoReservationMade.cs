public class NoReservationMade
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("No reservations made.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }
}
