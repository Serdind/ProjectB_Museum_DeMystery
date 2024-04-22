using Microsoft.Data.Sqlite;
using Spectre.Console;
using Newtonsoft.Json;

class Guide : Person
{
    private static int lastId = 0;
    public int Id;
    public string Name;
    private bool guideRunning = true;
    public Guide(string name, string qr) : base(qr)
    {
        Id = lastId++;
        Name = name;
    }
    public void ViewVisitorsTour()
    {
        Console.WriteLine("Which tour? (ID)");
        int tourID = Convert.ToInt32(Console.ReadLine());

        Tours.OverviewVisitorsTour(tourID);
    
        Console.WriteLine("Add visitor(A)\nRemove visitor(R)\nQuit (Q)");
        string option = Console.ReadLine();

        if (option.ToLower() == "a")
        {
            AddVisistorToTour(tourID);
        }
        else if (option.ToLower() == "r")
        {

        }
        else if (option.ToLower() == "q")
        {
            guideRunning = false;
        }
        else
        {
            Console.WriteLine("Wrong input. Try again.");
        }
    }

    public bool AddVisistorToTour(int tourID)
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
                Console.WriteLine("QR visitor:");
                string qr = Console.ReadLine();

                Visitor visitor = new Visitor(tourID, qr);
                
                visitor.Reservate(tourID, visitor);

                return true;
            }
            else
            {
                Console.WriteLine("Tour not found.");
                return false;
            }
        }
        return false;
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
                    string timeOnly = tour.Date.ToString("HH:mm");
                    string dateOnly = tour.Date.ToShortDateString();
                    int remainingSpots = Tours.maxParticipants - tour.ReservedVisitors.Count;
                    
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
            });

        Console.WriteLine("View visitors(V)\nQuit (Q)");
        string option = Console.ReadLine();

        if (option.ToLower() == "v")
        {
            ViewVisitorsTour();
        }
        else if (option.ToLower() == "q")
        {
            guideRunning = false;
        }
        else
        {
            Console.WriteLine("Wrong input. Try again.");
        }
    }
}