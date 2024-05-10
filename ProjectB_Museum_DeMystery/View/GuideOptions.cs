public class GuideOptions : View
{
    public static string Options()
    {
        Console.WriteLine("Add visitor(A)\nRemove visitor(R)");
        return ReadLineString();
    }

    public static void StartTour(GuidedTour tour)
    {
        string message = $"The tour has been started:" +
                        $"Tour: {tour.Name}\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Language: {tour.Language}\n";

        Console.WriteLine(message);
    }

    public static string ViewTours()
    {
        Console.WriteLine("My tours(M)");
        return ReadLineString();
    }
}