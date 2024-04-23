public class MessageTourReservation
{
    public static string ShowMessage(GuidedTour tour)
    {
        string message = $"Reservation successful. You have reserved the following tour:\n" +
                        $"Tour: {tour.Name}\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Language: {tour.Language}\n";
        
        return message;
    }

    public static string ViewReservation(GuidedTour tour)
    {
        string message = $"Tour: {tour.Name}\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Language: {tour.Language}\n";
        
        return message;
    }
}