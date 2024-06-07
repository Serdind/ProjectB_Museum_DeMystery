public class CodeNotValid : View
{
    
    public static void Show()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Code is not valid.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}