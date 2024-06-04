public class MaxReservation : View
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        Console.Clear();
        museum.WriteLine("You already made a reservation for today.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static void GuideShow()
    {
        museum.WriteLine("Visitor already made a reservation for today.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }
}
