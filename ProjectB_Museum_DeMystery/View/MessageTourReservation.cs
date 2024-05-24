public class MessageTourReservation
{
    public static void ShowMessage(GuidedTour tour)
    {
        string message = $"Reservation successful. You have reserved the following tour:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 40 min\n" +
                        $"Language: {tour.Language}\n" +
                        $"Guide: {tour.NameGuide}\n";

        Console.WriteLine(message);
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void ViewReservation(GuidedTour tour)
    {
        string message = $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 40 min\n" +
                        $"Language: {tour.Language}\n" +
                        $"Guide: {tour.NameGuide}\n";

        Console.WriteLine(message);
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void ViewStart(GuidedTour tour)
    {
        string message = $"Tour has been started:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Language: {tour.Language}\n" +
                        $"Guide: {tour.NameGuide}\n";

        Console.WriteLine(message);
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }

    public static void TourAdded()
    {
        Console.WriteLine("Tour succesfully added.");
        Console.WriteLine("Press any key to continue...\n");
        Console.ReadKey(true);
    }
}
