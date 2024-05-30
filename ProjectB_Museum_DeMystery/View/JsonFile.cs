public class JsonFile : View
{
    private static IMuseum museum = Program.Museum;
    public static void RemovedToursDoesNotExist()
    {
        museum.WriteLine("The removedTours.json file does not exist.");
    }
}