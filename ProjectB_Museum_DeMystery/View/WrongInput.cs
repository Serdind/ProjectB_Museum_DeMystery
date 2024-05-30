public class WrongInput
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("Wrong input. Try again.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }
}
