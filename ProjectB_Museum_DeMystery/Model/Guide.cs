using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;

public class Guide : Person
{
    private static int lastId = 0;
    [JsonPropertyName("Id")]
    public int Id;
    [JsonPropertyName("Name")]
    public string Name;
    
    public Guide(string name, string qr) : base(qr)
    {
        Id = lastId++;
        Name = name;
    }

    public bool AddVisistorToTour(int tourID, string qr)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Status == true)
            {
                Visitor visitor = new Visitor(tourID, qr);
                
                visitor.Reservate(tourID, visitor);

                return true;
            }
            else
            {
                TourNotFound.Show();
                return false;
            }
        }
        return false;
    }

    public bool RemoveVisitorFromTour(int tourID, string visitorQR)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        
        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName1 = "visitors.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

        List<Visitor> visitors;
        if (File.Exists(filePath1))
        {
            string json = File.ReadAllText(filePath1);
            visitors = JsonConvert.DeserializeObject<List<Visitor>>(json);
        }
        else
        {
            Console.WriteLine("Visitors data file not found.");
            return false;
        }

        var visitorToRemove = visitors.FirstOrDefault(v => v.QR == visitorQR);
        if (visitorToRemove != null)
        {
            visitors.Remove(visitorToRemove);
            Console.WriteLine("Visitor succesfully removed from tour.");
            File.WriteAllText(filePath1, JsonConvert.SerializeObject(visitors));
        }
        else
        {
            Console.WriteLine("Visitor not found in visitors list.");
            return false;
        }

        List<GuidedTour> tours;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);
        }
        else
        {
            Console.WriteLine("Tours data file not found.");
            return false;
        }

        var tour = tours.FirstOrDefault(t => t.ID == tourID);
        if (tour != null)
        {
            var reservedVisitor = tour.ReservedVisitors.FirstOrDefault(v => v.QR == visitorQR);
            if (reservedVisitor != null)
            {
                tour.ReservedVisitors.Remove(reservedVisitor);
                File.WriteAllText(filePath, JsonConvert.SerializeObject(tours));
                return true;
            }
            else
            {
                Console.WriteLine("Visitor not found in the reserved list for this tour.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Tour not found.");
            return false;
        }
    }

    public void ViewTours(string guideName)
    {
        DateTime today = DateTime.Today;
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        string jsonData = File.ReadAllText(filePath);
        
        List<GuidedTour> tours = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);
        
        List<GuidedTour> guideTours = tours.FindAll(tour => tour.NameGuide == guideName && tour.Date.Date == today);
        
        var table = new Table().LeftAligned();
        AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Top)
            .Start(ctx =>
            {
                table.AddColumn("ID");
                table.AddColumn("Name");
                table.AddColumn("Date");
                table.AddColumn("Time");
                table.AddColumn("StartingPoint");
                table.AddColumn("EndPoint");
                table.AddColumn("Language");
                table.AddColumn("Remaining spots");
                
                foreach (var tour in guideTours)
                {
                    if (tour.Date.Date == today.Date && tour.Date.TimeOfDay >= DateTime.Now.TimeOfDay && tour.Status)
                    {
                        string timeOnly = tour.Date.ToString("HH:mm");
                        string dateOnly = tour.Date.ToShortDateString();
                        int remainingSpots = Tour.maxParticipants - tour.ReservedVisitors.Count;
                        
                        table.AddRow(
                            tour.ID.ToString(),
                            tour.Name,
                            dateOnly,
                            timeOnly,
                            GuidedTour.StartingPoint,
                            GuidedTour.EndPoint,
                            tour.Language,
                            remainingSpots.ToString()
                        );
                        
                        ctx.Refresh();
                    }
                }
            });
        GuideController guideController= new GuideController();
        guideController.OptionsGuide(guideTours, Tour.guide);
    }

    public void StartTour(int tourID)
    {
        DateTime currentDate = DateTime.Now;
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= DateTime.Now.TimeOfDay && tour.Status)
            {
                tour.Status = false;
                Tour.SaveToursToFile(filePath, tours);
                MessageTourReservation.ViewStart(tour);
            }
            else
            {
                Console.WriteLine("Tour not found!");
            }
        }
        else
        {
            TourNotAvailable.Show();
        }
    }
}