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

    public static bool AddVisistorToTour(int tourID)
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

            if (tour != null)
            {
                string qr = QRVisitor.WhichVisitorQr();

                Visitor visitor = new Visitor(tourID, qr);
                
                Visitor.Reservate(tourID, visitor);

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

    public static void ViewTours(string guideName)
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
                table.AddColumn("Status");
                
                foreach (var tour in guideTours)
                {
                    string timeOnly = tour.Date.ToString("HH:mm");
                    string dateOnly = tour.Date.ToShortDateString();
                    int remainingSpots = Tour.maxParticipants - tour.ReservedVisitors.Count;
                    string status = tour.Status ? "Active" : "Inactive";
                    
                    table.AddRow(
                        tour.ID.ToString(),
                        tour.Name,
                        dateOnly,
                        timeOnly,
                        GuidedTour.StartingPoint,
                        GuidedTour.EndPoint,
                        tour.Language,
                        remainingSpots.ToString(),
                        status
                    );
                    
                    ctx.Refresh();
                }
            });

        GuideController.OptionsGuide();
    }

    public static void StartTour(int tourID)
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

            if (tour != null)
            {
                MessageTourReservation.ViewStart(tour);
            }
            else
            {
                TourNotFound.Show();
            }
        }
        else
        {
            TourNotAvailable.Show();
        }
    }
}