public class CodeNotValid : View
{
    private static IMuseum museum = Program.Museum;
    public static void Show()
    {
        museum.WriteLine("Code is not valid.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }
}