using System.Text.Json.Serialization;
using Newtonsoft.Json;
public class Visitor : Person
{
    private static int lastId = 1;
    [JsonPropertyName("Id")]
    public int Id;
    [JsonPropertyName("TourId")]
    public int TourId;

    public Visitor(int tourId, string qr) : base(qr)
    {
        Id = lastId++;
        TourId = tourId;
    }

    public static bool Reservate(int tourID, Visitor visitor)
    {
        if (ReservationMade(visitor.QR))
        {
            MaxReservation.Show();
            return false;
        }

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null)
            {
                if (Tour.maxParticipants > tour.ReservedVisitors.Count())
                {
                    Tour.AddVisitorToJSON(tourID, visitor.QR);

                    tour.ReservedVisitors.Add(visitor);
                    visitor.TourId = tour.ID;
                    string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
                    string fileName1 = "visitors.json";
                    string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

                    if (File.Exists(filePath1))
                    {
                        string json1 = File.ReadAllText(filePath1);
                        var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json1);

                        var v = visitors.FirstOrDefault(t => t.QR == visitor.QR);

                        visitor.Id = v.Id;
                    }

                    string updatedJson = JsonConvert.SerializeObject(tours, Formatting.Indented);

                    File.WriteAllText(filePath, updatedJson);

                    MessageTourReservation.ShowMessage(tour);
                    return true;
                }
                else
                {
                    TourFull.Show();
                }
            }
            else
            {
                TourNotAvailable.Show();
            }
        }

        return false;
    }

    public static bool ViewReservationsMade(string qr)
    {
        List<Visitor> visitors = Tour.LoadVisitorsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        if (visitor != null)
        {
            List<GuidedTour> tours = Tour.LoadToursFromFile();

            GuidedTour tour = tours.FirstOrDefault(t => t.ID == visitor.TourId);

            MessageTourReservation.ViewReservation(tour);
            return true;
        }
        NoReservationMade.Show();
        return false;
    }

    public static bool ReservationMade(string qr)
    {
        List<Visitor> visitors = Tour.LoadVisitorsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        return visitor != null;
    }

    public static void CancelReservation(Visitor visitor)
    {
        List<GuidedTour> tours = Tour.LoadToursFromFile();
        List<Visitor> visitors = Tour.LoadVisitorsFromFile();

        if (!ReservationMade(visitor.QR))
        {
            NoReservationMade.Show();
            return;
        }
        ViewReservationsMade(visitor.QR);
        VisitorController.ReservationCancel(tours, visitors, visitor);
        visitor.TourId = 0;
    }
}