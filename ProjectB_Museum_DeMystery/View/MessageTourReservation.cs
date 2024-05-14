public class MessageTourReservation
{
    public static string ShowMessage(GuidedTour tour)
    {
        string message = $"Reservation successful. You have reserved the following tour:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 20 min\n" +
                        $"Language: {tour.Language}\n";

        return message;
    }

    public static void ViewReservation(GuidedTour tour)
    {
        string message = $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 20 min\n" +
                        $"Language: {tour.Language}\n";

        Console.WriteLine(message);
    }

    public static void ViewStart(GuidedTour tour)
    {
        string message = $"Tour has been started:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Language: {tour.Language}\n";

        Console.WriteLine(message);
    }

    public static void TourAdded()
    {
        Console.WriteLine("Tour succesfully added.");
    }
}
