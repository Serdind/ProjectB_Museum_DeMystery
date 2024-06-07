public class WrongInput
{
    
    public static void Show()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("Wrong input. Try again.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}
