using Microsoft.Data.Sqlite;
using Spectre.Console;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Visitor : Person
{
    
    private static int lastId = 1;
    public int Id;
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
            Console.WriteLine("You already made a reservation for today.");
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
                if (Tours.maxParticipants > tour.ReservedVisitors.Count())
                {
                    Tours.AddVisitorToJSON(tourID, visitor.QR);

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

                    string message = $"Reservation successful. You have reserved the following tour:\n" +
                                    $"Tour: {tour.Name}\n" +
                                    $"Date: {tour.Date.ToShortDateString()}\n" +
                                    $"Time: {tour.Date.ToString("HH:mm")}\n" +
                                    $"Language: {tour.Language}\n";
                    Console.WriteLine(message);
                    return true;
                }
                else
                {
                    Console.WriteLine("Tour is full.");
                }
            }
            else
            {
                Console.WriteLine("Tour is not available.");
            }
        }

        return false;
    }

    public bool ViewReservationsMade(string qr)
    {
        List<Visitor> visitors = Tours.LoadVisitorsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        if (visitor != null)
        {
            List<GuidedTour> tours = Tours.LoadToursFromFile();

            GuidedTour tour = tours.FirstOrDefault(t => t.ID == visitor.TourId);

            if (tour != null)
            {
                string message = $"Tour: {tour.Name}\n" +
                                $"Date: {tour.Date.ToShortDateString()}\n" +
                                $"Time: {tour.Date.ToString("HH:mm")}\n" +
                                $"Language: {tour.Language}\n";

                Console.WriteLine(message);
                return true;
            }
        }
        return false;
    }

    public bool ReservationMade(string qr)
    {
        List<Visitor> visitors = Tours.LoadVisitorsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        return visitor != null;
    }

    public void CancelReservation(Visitor visitor)
    {
        List<GuidedTour> tours = Tours.LoadToursFromFile();
        List<Visitor> visitors = Tours.LoadVisitorsFromFile();

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName1 = "visitors.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

        if (!visitor.ReservationMade(visitor.QR))
        {
            Console.WriteLine("No reservations made.");
            return;
        }

        Console.WriteLine("Are you sure you want to cancel your reservation? (Y/N)");
        string confirmation = Console.ReadLine().ToLower();

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
            
            Console.WriteLine("Reservation cancelled successfully.");
        }
        else if (confirmation == "n")
        {
            Console.WriteLine("Reservation cancellation cancelled.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
        }
    }
}