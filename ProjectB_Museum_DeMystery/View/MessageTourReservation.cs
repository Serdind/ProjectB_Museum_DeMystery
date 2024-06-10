public class MessageTourReservation
{
    public static void ShowMessage(GuidedTour tour)
    {
        IMuseum museum = Program.Museum;
        string message = $"Reservation successful. You have reserved the following tour:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 40 minutes\n" +
                        $"Language: {tour.Language}\n" +
                        $"Name of guide: {tour.NameGuide}\n";

        museum.WriteLine("");
        museum.WriteLine(message);
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void ViewReservation(GuidedTour tour)
    {
        IMuseum museum = Program.Museum;
        string message = $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 40 minutes\n" +
                        $"Language: {tour.Language}\n" +
                        $"Name of guide: {tour.NameGuide}\n";
                        
        museum.WriteLine("");
        museum.WriteLine("Here are the details of your reservation:\n");
        museum.WriteLine(message);
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void ViewStart(GuidedTour tour)
    {
        IMuseum museum = Program.Museum;
        string message = $"Tour has been started:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Duration: {tour.Date.ToString("HH:mm")}\n" +
                        $"Language: {tour.Language}\n" +
                        $"Name of guide: {tour.NameGuide}\n";

        museum.WriteLine("");
        museum.WriteLine(message);
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void TourAdded()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Tour succesfully added.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }
}
