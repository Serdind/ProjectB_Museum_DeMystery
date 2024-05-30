public class MaxReservation : View
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("You already made a reservation for today.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }
}
