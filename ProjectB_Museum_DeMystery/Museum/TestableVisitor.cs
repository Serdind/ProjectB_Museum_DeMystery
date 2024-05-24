using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class TestableVisitor
{
    public readonly IMuseum Museum;

    public TestableVisitor(IMuseum museum)
    {
        Museum = museum;
    }

    public bool Reservate(int tourID, Visitor visitor)
    {
        if (ReservationMade(visitor.QR))
        {
            Museum.WriteLine("Maximum reservation limit reached.");
            return false;
        }

        DateTime currentDate = Museum.Now;
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (Museum.FileExists(filePath))
        {
            string json = Museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Status)
            {
                if (tour.MaxParticipants > tour.ReservedVisitors.Count())
                {
                    AddVisitorToTestJSON(tourID, visitor.QR);

                    tour.ReservedVisitors.Add(visitor);
                    visitor.TourId = tour.ID;
                    string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
                    string fileName1 = "visitorsTest.json";
                    string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

                    if (Museum.FileExists(filePath1))
                    {
                        string json1 = Museum.ReadAllText(filePath1);
                        var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json1);

                        var v = visitors.FirstOrDefault(t => t.QR == visitor.QR);

                        visitor.Id = v.Id;
                    }

                    string updatedJson = JsonConvert.SerializeObject(tours, Formatting.Indented);

                    Museum.WriteAllText(filePath, updatedJson);

                    string message = $"Reservation successful. You have reserved the following tour:\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 20 min\n" +
                        $"Language: {tour.Language}\n";

                    Museum.WriteLine(message);
                    return true;
                }
                else
                {
                    Museum.WriteLine("Tour is full.");
                }
            }
            else
            {
                Museum.WriteLine("Tour is not available.");
            }
        }
        return false;
    }

    public void AddVisitorToTestJSON(int tourId, string qr)
    {
        List<Visitor> existingVisitors = LoadVisitorsFromTestFile();

        Visitor newVisitor = new Visitor(tourId, qr);

        int nextId = existingVisitors.Count > 0 ? existingVisitors.Max(v => v.Id) + 1 : 1;

        newVisitor.Id = nextId;

        existingVisitors.Add(newVisitor);

        SaveVisitorToTestFile(existingVisitors);
    }

    public void SaveVisitorToTestFile(List<Visitor> visitors)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string json = JsonConvert.SerializeObject(visitors, Formatting.Indented);
        Museum.WriteAllText(filePath, json);
    }

    public bool ViewReservationsMade(string qr)
    {
        List<Visitor> visitors = LoadVisitorsFromTestFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        if (visitor != null)
        {
            List<GuidedTour> tours = LoadToursFromTestFile();

            GuidedTour tour = tours.FirstOrDefault(t => t.ID == visitor.TourId);

            string message = $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 20 min\n" +
                        $"Language: {tour.Language}\n";

            Museum.WriteLine(message);
            return true;
        }
        Museum.WriteLine("No reservation made.");
        return false;
    }

    public bool ReservationMade(string qr)
    {
        List<Visitor> visitors = LoadVisitorsFromTestFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        return visitor != null;
    }

    public void CancelReservation(Visitor visitor)
    {
        List<GuidedTour> tours = LoadToursFromTestFile();
        List<Visitor> visitors = LoadVisitorsFromTestFile();

        if (!ReservationMade(visitor.QR))
        {
            Museum.WriteLine("No reservation made.");
            return;
        }
        ViewReservationsMade(visitor.QR);
        ReservationCancelTest(tours, visitors, visitor);
        visitor.TourId = 0;
    }

    public List<Visitor> LoadVisitorsFromTestFile()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (Museum.FileExists(filePath))
        {
            string json = Museum.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Visitor>>(json);
        }
        else
        {
            return new List<Visitor>();
        }
    }

    public List<GuidedTour> LoadToursFromTestFile()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (Museum.FileExists(filePath))
        {
            string json = Museum.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GuidedTour>>(json);
        }

        return new List<GuidedTour>();
    }

    public void ReservationCancelTest(List<GuidedTour> tours, List<Visitor> visitors, Visitor visitor)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName1 = "visitorsTest.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

        string confirmation = Museum.ReadLine();

        if (confirmation == "y")
        {
            foreach (var tour in tours)
            {
                tour.ReservedVisitors.RemoveAll(v => v.QR == visitor.QR);
            }

            visitors.RemoveAll(v => v.QR == visitor.QR);

            string toursJson = JsonConvert.SerializeObject(tours, Formatting.Indented);
            Museum.WriteAllText(filePath, toursJson);

            string visitorsJson = JsonConvert.SerializeObject(visitors, Formatting.Indented);
            Museum.WriteAllText(filePath1, visitorsJson);

            Museum.WriteLine("Reservation cancelled successfully.");
        }
        else if (confirmation == "n")
        {
            Museum.WriteLine("Reservation cancellation cancelled.");
        }
        else
        {
            Museum.WriteLine("Wrong input. Try again.");
        }
    }
}