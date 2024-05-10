using Spectre.Console;
using Newtonsoft.Json;

static class Tour
{
    public static readonly List<GuidedTour> guidedTour = new List<GuidedTour>();
    public static readonly List<DepartmentHead> admins = new List<DepartmentHead>();
    public static readonly List<Guide> guides = new List<Guide>();
    public static List<Visitor> visitors = new List<Visitor>();
    public static Guide guide = new Guide("Casper", "4892579");
    public static int maxParticipants = 13;

    public static void UpdateTours()
    {
        DateTime today = DateTime.Today;
        DateTime endDate = today.AddDays(7);
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
            
        if (File.Exists(filePath))
        {
            DateTime lastWriteTime = File.GetLastWriteTime(filePath).Date;
            
            if (lastWriteTime.Year < endDate.Year)
            {
                RemoveToursFromDate(lastWriteTime);
                ToursDay(today, endDate);
            }
        }
        else
        {
            ToursDay(today, endDate);
        }
    }

    public static void ToursDay(DateTime startDate, DateTime endDate)
    {
        DateTime currentDate = startDate.Date;

        while (currentDate <= endDate.Date)
        {
            if (currentDate.Date >= DateTime.Today.Date && currentDate.Date <= endDate.Date)
            {
                AddTour(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 11, 30, 0), "English", guide.Name));
                AddTour(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 00, 0), "Dutch", guide.Name));
                AddTour(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 30, 0), "English", guide.Name));
                AddTour(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 14, 00, 0), "Dutch", guide.Name));
                AddTour(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 14, 30, 0), "English", guide.Name));
                AddTour(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 15, 00, 0), "Dutch", guide.Name));
                AddTour(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 16, 00, 0), "English", guide.Name));
                AddTour(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 16, 30, 0), "Dutch", guide.Name));
                AddTour(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 17, 00, 0), "English", guide.Name));
            }
            currentDate = currentDate.AddDays(1);
        }

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        SaveToursToFile(filePath, guidedTour);
    }


    public static void AddTour(GuidedTour tour)
    {
        List<GuidedTour> tours = LoadToursFromFile();
        
        int newID = tours.Count > 0 ? tours.Max(t => t.ID) + 1 : 1;
        
        tour.ID = newID;
        tours.Add(tour);

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        
        SaveToursToFile(filePath, tours);
    }

    public static void SaveToursToFile(string filePath, List<GuidedTour> tours)
    {
        List<GuidedTour> existingTours;
        if (File.Exists(filePath))
        {
            string existingJson = File.ReadAllText(filePath);
            existingTours = JsonConvert.DeserializeObject<List<GuidedTour>>(existingJson);
        }
        else
        {
            existingTours = new List<GuidedTour>();
        }

        List<GuidedTour> updatedTours = new List<GuidedTour>(existingTours);

        foreach (var tour in tours)
        {
            var existingTour = updatedTours.FirstOrDefault(t => t.ID == tour.ID);
            if (existingTour != null)
            {
                existingTour.Name = tour.Name;
                existingTour.Date = tour.Date;
                existingTour.Language = tour.Language;
                existingTour.Status = tour.Status;
            }
            else
            {
                updatedTours.Add(tour);
            }
        }

        string updatedJson = JsonConvert.SerializeObject(updatedTours, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);
    }

    public static void OverviewTours(bool edit)
    {
        DateTime currentDate = DateTime.Today;
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (edit)
        {
            currentDate = currentDate.AddDays(1);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

                tours = tours.OrderBy(t => t.Date).ToList();

                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Date");
                table.AddColumn("Time");
                table.AddColumn("Language");
                table.AddColumn("Guide");
                table.AddColumn("Remaining spots");
                table.AddColumn("Status");

                foreach (var tour in tours)
                {
                    if (tour.Date.Date == currentDate.Date)
                    {
                        string timeOnly = tour.Date.ToString("HH:mm");
                        string dateOnly = tour.Date.ToShortDateString();
                        int remainingSpots = maxParticipants - tour.ReservedVisitors.Count;
                        string status = tour.Status ? "Active" : "Inactive";

                        table.AddRow(
                            tour.ID.ToString(),
                            dateOnly,
                            timeOnly,
                            tour.Language,
                            guide.Name,
                            remainingSpots.ToString(),
                            status
                        );
                    }
                }

                AnsiConsole.Render(table);
            }
            else
            {
                TourEmpty.Show();
            }
        }
        else
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

                tours = tours.OrderBy(t => t.Date).ToList();

                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Date");
                table.AddColumn("Time");
                table.AddColumn("Language");
                table.AddColumn("Guide");
                table.AddColumn("Remaining spots");
                table.AddColumn("Status");

                foreach (var tour in tours)
                {
                    if (tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= DateTime.Now.TimeOfDay && tour.Status)
                    {
                        string timeOnly = tour.Date.ToString("HH:mm");
                        string dateOnly = tour.Date.ToShortDateString();
                        int remainingSpots = maxParticipants - tour.ReservedVisitors.Count;
                        string status = tour.Status ? "Active" : "Inactive";

                        table.AddRow(
                            tour.ID.ToString(),
                            dateOnly,
                            timeOnly,
                            tour.Language,
                            guide.Name,
                            remainingSpots.ToString(),
                            status
                        );
                    }
                }

                AnsiConsole.Render(table);
            }
            else
            {
                TourEmpty.Show();
            }
        }
    }

    public static void OverviewRemovedTours()
    {
        DateTime currentDate = DateTime.Today;

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "removedTours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                TourEmpty.RemovedToursEmpty();
                return;
            }

            var tour = JsonConvert.DeserializeObject<List<GuidedTour>>(json).FirstOrDefault();

            if (tour == null)
            {
                TourNotFound.RemovedToursNotFound();
                return;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Date");
            table.AddColumn("Time");
            table.AddColumn("Language");
            table.AddColumn("Guide");
            table.AddColumn("Remaining spots");

            string timeOnly = tour.Date.ToString("HH:mm");
            string dateOnly = tour.Date.ToShortDateString();
            int remainingSpots = maxParticipants - tour.ReservedVisitors.Count;
            string status = tour.Status ? "Active" : "Inactive";

            table.AddRow(
                tour.ID.ToString(),
                tour.Name,
                dateOnly,
                timeOnly,
                tour.Language,
                tour.NameGuide,
                remainingSpots.ToString(),
                status
            );

            AnsiConsole.Render(table);
        }
        else
        {
            JsonFile.RemovedToursDoesNotExist();
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

    public static List<GuidedTour> LoadRemovedToursFromFile()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "removedTours.json";
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
        List<Visitor> existingVisitors = LoadVisitorsFromFile();

        Visitor newVisitor = new Visitor(tourId, qr);

        int nextId = existingVisitors.Count > 0 ? existingVisitors.Max(v => v.Id) + 1 : 1;

        newVisitor.Id = nextId;

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

    public static void OverviewVisitorsTour(int tourID)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "visitors.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json);
            
            visitors = visitors.Where(v => v.TourId == tourID).OrderBy(t => t.TourId).ToList();

            if (visitors.Any())
            {
                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Qr");

                foreach (var visitor in visitors)
                {
                    table.AddRow(
                        visitor.Id.ToString(),
                        visitor.QR.ToString()
                    );
                }

                AnsiConsole.Render(table);
            }
            else
            {
                TourEmpty.NoVisitorsInTour();
            }
        }
        else
        {
            TourEmpty.Show();
        }
    }
}