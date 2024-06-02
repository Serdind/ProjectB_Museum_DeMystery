using System.Text.Json.Serialization;
using Newtonsoft.Json;
public class Visitor : Person
{
    private static IMuseum museum = Program.Museum;
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

    public bool Reservate(int tourID, Visitor visitor)
    {
        if (ReservationMade(visitor.QR))
        {
            MaxReservation.Show();
            return false;
        }
        

        DateTime currentDate = museum.Now;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= museum.Now.TimeOfDay && tour.Status)
            {
                if (tour.MaxParticipants > tour.ReservedVisitors.Count())
                {
                    Tour.AddVisitorToJSON(tourID, visitor.QR);

                    tour.ReservedVisitors.Add(visitor);
                    visitor.TourId = tour.ID;
                    string filePath1 = Model<Visitor>.GetFileNameVisitors();

                    if (museum.FileExists(filePath1))
                    {
                        string json1 = museum.ReadAllText(filePath1);
                        var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json1);

                        var v = visitors.FirstOrDefault(t => t.QR == visitor.QR);

                        visitor.Id = v.Id;
                    }

                    string updatedJson = JsonConvert.SerializeObject(tours, Formatting.Indented);

                    museum.WriteAllText(filePath, updatedJson);

                    MessageTourReservation.ShowMessage(tour);
                    return true;
                }
                else
                {
                    TourFull.Show();
                    return false;
                }
            }
            else
            {
                TourNotAvailable.Show();
                return false;
            }
        }
        return false;
    }

    public bool ReservateByGuide(int tourID, Visitor visitor)
    {
        if (ReservationMade(visitor.QR))
        {
            MaxReservation.GuideShow();
            return false;
        }
        
        DateTime currentDate = museum.Now;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= museum.Now.TimeOfDay && tour.Status)
            {
                if (tour.MaxParticipants > tour.ReservedVisitors.Count())
                {
                    Tour.AddVisitorToJSON(tourID, visitor.QR);

                    tour.ReservedVisitors.Add(visitor);
                    visitor.TourId = tour.ID;
                    string filePath1 = Model<Visitor>.GetFileNameVisitors();

                    if (museum.FileExists(filePath1))
                    {
                        string json1 = museum.ReadAllText(filePath1);
                        var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json1);

                        var v = visitors.FirstOrDefault(t => t.QR == visitor.QR);

                        visitor.Id = v.Id;
                    }

                    string updatedJson = JsonConvert.SerializeObject(tours, Formatting.Indented);

                    museum.WriteAllText(filePath, updatedJson);

                    return true;
                }
                else
                {
                    TourFull.Show();
                    return false;
                }
            }
            else
            {
                TourNotAvailable.Show();
                return false;
            }
        }
        return false;
    }

    public bool ViewReservationsMade(string qr)
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

    public bool ReservationMade(string qr)
    {
        List<Visitor> visitors = Tour.LoadVisitorsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        return visitor != null;
    }

    public void CancelReservation(Visitor visitor)
    {
        List<GuidedTour> tours = Tour.LoadToursFromFile();
        List<Visitor> visitors = Tour.LoadVisitorsFromFile();
        VisitorController visitorController = new VisitorController();

        if (!ReservationMade(visitor.QR))
        {
            NoReservationMade.Show();
            return;
        }
        ViewReservationsMade(visitor.QR);
        visitorController.ReservationCancel(tours, visitors, visitor);
        visitor.TourId = 0;
    }
}