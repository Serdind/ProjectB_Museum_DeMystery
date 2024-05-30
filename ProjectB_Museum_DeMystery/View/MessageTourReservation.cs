public class MessageTourReservation
{
    private static IMuseum museum = Program.Museum;
    public static void ShowMessage(GuidedTour tour)
    {
        string message = $"Reservation successful. You have reserved the following tour:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 40 min\n" +
                        $"Language: {tour.Language}\n" +
                        $"Guide: {tour.NameGuide}\n";

        museum.WriteLine(message);
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static void ViewReservation(GuidedTour tour)
    {
        string message = $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 40 min\n" +
                        $"Language: {tour.Language}\n" +
                        $"Guide: {tour.NameGuide}\n";

        museum.WriteLine(message);
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static void ViewStart(GuidedTour tour)
    {
        string message = $"Tour has been started:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Language: {tour.Language}\n" +
                        $"Guide: {tour.NameGuide}\n";

        museum.WriteLine(message);
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }

    public static void TourAdded()
    {
        museum.WriteLine("Tour succesfully added.");
        museum.WriteLine("Press any key to continue...\n");
        museum.ReadKey();
    }
}
