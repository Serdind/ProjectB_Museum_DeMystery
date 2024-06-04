public class WrongInput
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("Wrong input. Try again.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }
}
