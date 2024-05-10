public static class TourController
{
    public static void ReservateTour(Visitor visitor)
    {
        Tour.OverviewTours(false);
        int tourID = TourId.WhichTourId();

        visitor.Reservate(tourID, visitor);
    }
}
using Spectre.Console;
using Newtonsoft.Json;

public static class VisitorController
{
    public static void ReservationCancel(List<GuidedTour> tours, List<Visitor> visitors, Visitor visitor)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName1 = "visitors.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);
        string confirmation = CancelReservationConfirmation.Options();

        if (confirmation == "y")
        {
            foreach (var tour in tours)
            {
                tour.ReservedVisitors.RemoveAll(v => v.QR == visitor.QR);
            }

            visitors.RemoveAll(v => v.QR == visitor.QR);

            string toursJson = JsonConvert.SerializeObject(tours, Formatting.Indented);
            File.WriteAllText(filePath, toursJson);

            string visitorsJson = JsonConvert.SerializeObject(visitors, Formatting.Indented);
            File.WriteAllText(filePath1, visitorsJson);

            CancelReservationConfirmation.ReservationCancelled();
        }
        else if (confirmation == "n")
        {
            CancelReservationConfirmation.ReservationCancelDenied();
        }
        else
        {
            WrongInput.Show();
        }
    }
}