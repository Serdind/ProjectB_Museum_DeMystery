public class TourFull : View
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("Tour is full.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }
}
