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
                        $"Name of guide: {tour.NameGuide}\n";

        museum.WriteLine(message);
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void ViewReservation(GuidedTour tour)
    {
        string message = $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 40 min\n" +
                        $"Language: {tour.Language}\n" +
                        $"Name of guide: {tour.NameGuide}\n";

        museum.WriteLine("Here are the details of your reservation:\n");
        museum.WriteLine(message);
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void ViewStart(GuidedTour tour)
    {
        string message = $"Tour has been started:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Duration: {tour.Date.ToString("HH:mm")}\n" +
                        $"Language: {tour.Language}\n" +
                        $"Name of guide: {tour.NameGuide}\n";

        museum.WriteLine(message);
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void TourAdded()
    {
        museum.WriteLine("Tour succesfully added.");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }
}
