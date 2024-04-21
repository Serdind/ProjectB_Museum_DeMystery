using Spectre.Console;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

static class Tours
{
    public static readonly List<GuidedTour> guidedTour = new List<GuidedTour>();
    public static readonly List<DepartmentHead> admins = new List<DepartmentHead>();
    public static readonly List<Guide> guides = new List<Guide>();
    public static List<Visitor> visitors = new List<Visitor>();
    public static Guide guide = new Guide("Casper", "4892579");
    public static int maxParticipants = 1;

    public static void UpdateTours()
    {
        DateTime today = DateTime.Today;
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
            
        if (File.Exists(filePath))
        {
            DateTime lastWriteTime = File.GetLastWriteTime(filePath).Date;
            
            if (lastWriteTime != today)
            {
                RemoveToursFromDate(lastWriteTime);
                
                ToursDay(today);
                ToursDay(today.AddDays(1));
            }
        }
        else
        {
            ToursDay(today);
            ToursDay(today.AddDays(1));
        }
    }

    public static void ToursDay(DateTime date)
    {
        if (date.Date == DateTime.Today.Date || date.Date == DateTime.Today.AddDays(1).Date)
        {
            AddTour(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 11, 30, 0), "English", guide.Name));
            AddTour(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 13, 00, 0), "Dutch", guide.Name));
            AddTour(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 13, 30, 0), "English", guide.Name));
            AddTour(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 14, 00, 0), "Dutch", guide.Name));
            AddTour(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 14, 30, 0), "English", guide.Name));
            AddTour(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 15, 00, 0), "Dutch", guide.Name));
            AddTour(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 16, 00, 0), "English", guide.Name));
            AddTour(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 16, 30, 0), "Dutch", guide.Name));
            AddTour(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 17, 00, 0), "English", guide.Name));
        }

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        SaveToursToFile(filePath);
    }

    public static void AddTour(GuidedTour tour)
    {
        guidedTour.Add(tour);
    }

    public static void SaveToursToFile(string filePath)
    {
        string json = JsonConvert.SerializeObject(guidedTour, Formatting.Indented);

        File.WriteAllText(filePath, json);
    }

    public static void OverviewTours(bool edit)
    {
        DateTime currentDate = DateTime.Today;

        if (edit)
        {
            currentDate = DateTime.Today.AddDays(1);
        }

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Date");
            table.AddColumn("Time");
            table.AddColumn("Language");
            table.AddColumn("Guide");
            table.AddColumn("Visitors");

            foreach (var tour in tours)
            {
                if (tour.Date.Date == currentDate.Date)
                {
                    string timeOnly = tour.Date.ToString("HH:mm");
                    string dateOnly = tour.Date.ToShortDateString();

                    table.AddRow(
                        tour.ID.ToString(),
                        tour.Name,
                        dateOnly,
                        timeOnly,
                        tour.Language,
                        guide.Name,
                        tour.ReservedVisitors.Count().ToString()
                    );
                }
            }

            AnsiConsole.Render(table);
        }
        else
        {
            Console.WriteLine("Tours JSON file not found.");
        }
    }

    private static void RemoveToursFromDate(DateTime date)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            List<GuidedTour> tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            tours.RemoveAll(tour => tour.Date.Date == date.Date);

            string updatedJson = JsonConvert.SerializeObject(tours, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
        }
    }

    public static List<GuidedTour> LoadToursFromFile()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GuidedTour>>(json);
        }

        return new List<GuidedTour>();
    }
    public static void AddAdminToJSON()
    {
        AddAdmin(new DepartmentHead("Frans", "6457823"));

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "admins.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        SaveAdminToFile(filePath);
    }

    public static void AddAdmin(DepartmentHead departmentHead)
    {
        admins.Add(departmentHead);
    }

    public static void SaveAdminToFile(string filePath)
    {
        string json = JsonConvert.SerializeObject(admins, Formatting.Indented);

        File.WriteAllText(filePath, json);
    }

    public static List<DepartmentHead> LoadAdminsFromFile()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "admins.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<DepartmentHead>>(json);
        }
        else
        {
            return new List<DepartmentHead>();
        }
    }

    public static void AddGuideToJSON()
    {
        AddGuide(new Guide("Casper", "4892579"));

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "guides.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        SaveGuideToFile(filePath);
    }

    public static void AddGuide(Guide guide)
    {
        guides.Add(guide);
    }

    public static void SaveGuideToFile(string filePath)
    {
        string json = JsonConvert.SerializeObject(guides, Formatting.Indented);

        File.WriteAllText(filePath, json);
    }

    public static List<Guide> LoadGuidesFromFile()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "guides.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Guide>>(json);
        }
        else
        {
            return new List<Guide>();
        }
    }
    
    public static void AddVisitorToJSON(int tourId, string qr)
    {
        int highestId;
        List<Visitor> existingVisitors = LoadVisitorsFromFile();

        if (existingVisitors.Count > 0)
        {
            highestId = existingVisitors.Max(v => v.Id);
        }
        else
        {
            highestId = 0;
        }

        Visitor newVisitor = new Visitor(tourId, qr);
        newVisitor.Id = highestId + 1;

        existingVisitors.Add(newVisitor);

        SaveVisitorToFile(existingVisitors);
    }

    public static List<Visitor> LoadVisitorsFromFile()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "visitors.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Visitor>>(json);
        }
        else
        {
            return new List<Visitor>();
        }
    }

    public static void AddVisitor(Visitor visitor)
    {
        visitors.Add(visitor);
    }

    public static void SaveVisitorToFile(List<Visitor> visitors)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "visitors.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string json = JsonConvert.SerializeObject(visitors, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static void ReservateTour(Visitor visitor)
    {
        OverviewTours(false);
        Console.WriteLine("Which tour? (ID)");
        int tourID = Convert.ToInt32(Console.ReadLine());

        visitor.Reservate(tourID, visitor);
    }
}