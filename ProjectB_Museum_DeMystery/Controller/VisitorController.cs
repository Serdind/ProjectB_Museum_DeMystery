using Spectre.Console;
using Newtonsoft.Json;

public class VisitorController
{
    private static IMuseum museum = Program.Museum;
    public void ReservationCancel(List<GuidedTour> tours, List<Visitor> visitors, Visitor visitor)
    {
        string filePath = Model<GuidedTour>.GetFileNameTours();

        string filePath1 = Model<Visitor>.GetFileNameVisitors();

        bool wrongInputShown = false;

        while (true)
        {
            string confirmation = CancelReservationConfirmation.Options();

            if (confirmation.ToLower() == "y" || confirmation.ToLower() == "yes")
            {
                foreach (var tour in tours)
                {
                    tour.ReservedVisitors.RemoveAll(v => v.QR == visitor.QR);
                }

                visitors.RemoveAll(v => v.QR == visitor.QR);

                string toursJson = JsonConvert.SerializeObject(tours, Formatting.Indented);
                museum.WriteAllText(filePath, toursJson);

                string visitorsJson = JsonConvert.SerializeObject(visitors, Formatting.Indented);
                museum.WriteAllText(filePath1, visitorsJson);

                CancelReservationConfirmation.ReservationCancelled();
                break;
            }
            else if (confirmation.ToLower() == "n" || confirmation.ToLower() == "no")
            {
                CancelReservationConfirmation.ReservationCancelDenied();
                break;
            }
            else
            {
                if (!wrongInputShown)
                {
                    WrongInput.Show();
                    wrongInputShown = true;
                }
            }
        }
    }
}