public class TourNotAvailable : View
{
    public static void Show()
    {
        Console.WriteLine("Tour is not available.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }
}
