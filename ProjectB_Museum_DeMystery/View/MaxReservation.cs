public class MaxReservation : View
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("You already made a reservation for today.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void GuideShow()
    {
        museum.WriteLine("Visitor already made a reservation for today.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }
}
