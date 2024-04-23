public class TourId : View
{
    public static int WhichTourId()
    {
        Console.WriteLine("Which tour? (ID)");
        return ReadLineInt();
    }
}