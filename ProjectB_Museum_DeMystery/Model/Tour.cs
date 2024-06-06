using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;

public static class Tour
{
    public static readonly List<GuidedTour> guidedTour = new List<GuidedTour>();
    public static readonly List<DepartmentHead> admins = new List<DepartmentHead>();
    public static readonly List<Guide> guides = new List<Guide>();
    public static List<Visitor> visitors = new List<Visitor>();
    public static Guide guide = new Guide("Casper", "4892579");
    private static DateTime lastCheckDate = DateTime.MinValue;

    private static IMuseum museum = Program.Museum;

    public static void UpdateTours()
    {
        ClearVisitorFileIfNewDay();

        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            List<GuidedTour> existingTours = LoadToursFromFile();
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);

            for (int i = 0; i < existingTours.Count; i++)
            {
                GuidedTour tour = existingTours[i];

                if (i < 9)
                {
                    tour.Date = new DateTime(today.Year, today.Month, today.Day, tour.Date.Hour, tour.Date.Minute, tour.Date.Second);
                }
                else
                {
                    tour.Date = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, tour.Date.Hour, tour.Date.Minute, tour.Date.Second);
                }
            }

            SaveToursToFile(filePath, existingTours);
        }
        else
        {
            DateTime today = DateTime.Today;
            List<GuidedTour> defaultToursToday = GenerateDefaultToursForDay(today);
            List<GuidedTour> defaultToursTomorrow = GenerateDefaultToursForDay(today.AddDays(1));
            List<GuidedTour> defaultTours = defaultToursToday.Concat(defaultToursTomorrow).ToList();
            SaveToursToFile(filePath, defaultTours);
        }
    }

    private static List<GuidedTour> GenerateDefaultToursForDay(DateTime date)
    {
        return new List<GuidedTour>
        {
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 11, 30, 0), "English", guide.Name),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 13, 00, 0), "Dutch", guide.Name),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 13, 30, 0), "English", guide.Name),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 14, 00, 0), "Dutch", guide.Name),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 14, 30, 0), "English", guide.Name),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 15, 00, 0), "Dutch", guide.Name),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 16, 00, 0), "English", guide.Name),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 16, 30, 0), "Dutch", guide.Name),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 17, 00, 0), "English", guide.Name)
        };
    }

    public static bool ToursExistForTimeAndLanguage(DateTime time, string language, List<GuidedTour> tours)
    {
        return tours.Any(t => t.Date == time && t.Language == language);
    }

    public static void AddTour(GuidedTour tour, List<GuidedTour> tours)
    {
        int newID = tours.Count > 0 ? tours.Max(t => t.ID) + 1 : 1;

        tour.ID = newID;
        tours.Add(tour);
    }

    public static void SaveToursToFile(string filePath, List<GuidedTour> tours)
    {
        List<GuidedTour> existingTours;
        if (museum.FileExists(filePath))
        {
            string existingJson = museum.ReadAllText(filePath);
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
        museum.WriteAllText(filePath, updatedJson);
    }

    public static bool OverviewTours(bool edit)
    {
        DateTime currentDate = DateTime.Today;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (edit)
        {
            Console.Clear();
            bool validDateSelected = false;
            DateTime selectedDate = DateTime.MinValue;

            while (!validDateSelected)
            {
                AdminOptions.BackOption();
                string dateString = TourInfo.WhichDate();
                if (dateString.ToLower() == "b" || dateString.ToLower() == "back")
                {
                    return false;
                }
                if (DateTime.TryParseExact(dateString, "d-M-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate))
                {
                    validDateSelected = true;
                }
                else
                {
                    TourInfo.InvalidDate();
                }
            }

            if (museum.FileExists(filePath))
            {
                Console.Clear();
                string json = museum.ReadAllText(filePath);
                var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

                tours = tours.OrderBy(t => t.Date).ToList();
                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Date");
                table.AddColumn("Time");
                table.AddColumn("Duration");
                table.AddColumn("Language");
                table.AddColumn("Guide");
                table.AddColumn("Remaining spots");
                table.AddColumn("Status");
                foreach (var tour in tours)
                {
                    if (tour.Date.Date == selectedDate.Date)
                    {
                        string timeOnly = tour.Date.ToString("HH:mm");
                        string dateOnly = tour.Date.ToShortDateString();
                        int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                        string status = tour.Status ? "Active" : "Inactive";
                        table.AddRow(
                            tour.ID.ToString(),
                            dateOnly,
                            timeOnly,
                            "40 minutes",
                            tour.Language,
                            guide.Name,
                            remainingSpots.ToString(),
                            status
                        );
                    }
                }
                AnsiConsole.Render(table);
                return true;
            }
            else
            {
                TourEmpty.Show();
                return false;
            }
        }
        else
        {
            if (museum.FileExists(filePath))
            {
                string json = museum.ReadAllText(filePath);
                var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);
                tours = tours.OrderBy(t => t.Date).ToList();
                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Date");
                table.AddColumn("Time");
                table.AddColumn("Duration");
                table.AddColumn("Language");
                table.AddColumn("Guide");
                table.AddColumn("Remaining spots");
                table.AddColumn("Status");
                bool anyToursToday = false;
                foreach (var tour in tours)
                {
                    if (tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= DateTime.Now.TimeOfDay && tour.Status)
                    {
                        anyToursToday = true;
                        string timeOnly = tour.Date.ToString("HH:mm");
                        string dateOnly = tour.Date.ToShortDateString();
                        int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                        string status = tour.Status ? "Active" : "Inactive";
                        table.AddRow(
                            tour.ID.ToString(),
                            dateOnly,
                            timeOnly,
                            "40 minutes",
                            tour.Language,
                            guide.Name,
                            remainingSpots.ToString(),
                            status
                        );
                    }
                }
                if (anyToursToday)
                {
                    AnsiConsole.Render(table);
                    return true;
                }
                else
                {
                    TourInfo.NoToursToday();
                    return false;
                }
            }
            else
            {
                TourEmpty.Show();
            }
            return false;
        }
    }

    public static bool OverviewToursTomorrow()
    {
        DateTime currentDate = DateTime.Today;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        DateTime tomorrowDate = currentDate.AddDays(1);

        if (museum.FileExists(filePath))
        {
            Console.Clear();
            string json = museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            tours = tours.OrderBy(t => t.Date).ToList();
            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("ID");
            table.AddColumn("Date");
            table.AddColumn("Time");
            table.AddColumn("Duration");
            table.AddColumn("Language");
            table.AddColumn("Guide");
            table.AddColumn("Remaining spots");
            table.AddColumn("Status");

            foreach (var tour in tours)
            {
                if (tour.Date.Date == tomorrowDate.Date)
                {
                    string timeOnly = tour.Date.ToString("HH:mm");
                    string dateOnly = tour.Date.ToShortDateString();
                    int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                    string status = tour.Status ? "Active" : "Inactive";
                    table.AddRow(
                        tour.ID.ToString(),
                        dateOnly,
                        timeOnly,
                        "40 minutes",
                        tour.Language,
                        guide.Name,
                        remainingSpots.ToString(),
                        status
                    );
                }
            }

            AnsiConsole.Render(table);
            return true;
        }
        else
        {
            TourEmpty.Show();
            return false;
        }
    }

    public static bool OverviewToursEdit()
    {
        DateTime tomorrowDate = museum.Today.AddDays(1);
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tomorrowTours = tours.Where(t => t.Date.Date == tomorrowDate.Date).OrderBy(t => t.Date).ToList();

            if (tomorrowTours.Any())
            {
                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("Time");
                table.AddColumn("Duration");
                table.AddColumn("Language");
                table.AddColumn("Guide");
                table.AddColumn("Status");

                foreach (var tour in tomorrowTours)
                {
                    string timeOnly = tour.Date.ToString("HH:mm");
                    string dateOnly = tour.Date.ToShortDateString();
                    int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                    string status = tour.Status ? "Active" : "Inactive";

                    table.AddRow(
                        timeOnly,
                        "40 minutes",
                        tour.Language,
                        guide.Name,
                        status
                    );
                }

                AnsiConsole.Render(table);
                return true;
            }
            else
            {
                TourInfo.NoTours();
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static List<GuidedTour> LoadToursFromFile()
    {
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GuidedTour>>(json);
        }

        return new List<GuidedTour>();
    }

    public static void AddAdminToJSON()
    {
        AddAdmin(new DepartmentHead("Frans", "6457823"));

        string filePath = Model<DepartmentHead>.GetFileNameAdmins();
        SaveAdminToFile(filePath);
    }

    public static void AddAdmin(DepartmentHead departmentHead)
    {
        admins.Add(departmentHead);
    }

    public static void SaveAdminToFile(string filePath)
    {
        string json = JsonConvert.SerializeObject(admins, Formatting.Indented);

        museum.WriteAllText(filePath, json);
    }

    public static List<DepartmentHead> LoadAdminsFromFile()
    {
        string filePath = Model<DepartmentHead>.GetFileNameAdmins();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
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

        string filePath = Model<Guide>.GetFileNameGuides();
        SaveGuideToFile(filePath);
    }

    public static void AddGuide(Guide guide)
    {
        guides.Add(guide);
    }

    public static void SaveGuideToFile(string filePath)
    {
        string json = JsonConvert.SerializeObject(guides, Formatting.Indented);

        museum.WriteAllText(filePath, json);
    }

    public static List<Guide> LoadGuidesFromFile()
    {
        string filePath = Model<Guide>.GetFileNameGuides();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
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
        string filePath = Model<Visitor>.GetFileNameVisitors();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
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
        string filePath = Model<Visitor>.GetFileNameVisitors();

        string json = JsonConvert.SerializeObject(visitors, Formatting.Indented);
        museum.WriteAllText(filePath, json);
    }

    public static bool OverviewVisitorsTour(int tourID)
    {
        string filePath = Model<Visitor>.GetFileNameVisitors();
        
        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
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
                return true;
            }
            else
            {
                TourEmpty.NoVisitorsInTour();
                return false;
            }
        }
        else
        {
            TourEmpty.Show();
            return false;
        }
    }

    public static void CreateEmptyJsonFile(string filePath)
    {
        if (!museum.FileExists(filePath))
        {
            museum.WriteAllText(filePath, "[]");
        }
    }

    private static void ClearVisitorFileIfNewDay()
    {
        DateTime today = DateTime.Today;
        if (lastCheckDate.Date != today)
        {
            string visitorFilePath = Model<Visitor>.GetFileNameVisitors();
            CreateEmptyJsonFile(visitorFilePath);

            lastCheckDate = today;
        }
    }

    public static bool ToursExistForTime(DateTime time, List<GuidedTour> tours)
    {
        foreach (GuidedTour tour in tours)
        {
            if (tour.Date.Hour == time.Hour && tour.Date.Minute == time.Minute)
            {
                return true;
            }
        }
        return false;
    }

    public static void SelectedTour(string time, DateTime selectedDate)
    {
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string jsonData = museum.ReadAllText(filePath);
            List<GuidedTour> toursFile = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);

            var table = new Table().LeftAligned();
            table.AddColumn("Time");
            table.AddColumn("Language");
            table.AddColumn("Status");

            foreach (var tour in toursFile)
            {
                if (tour.Date.Date == selectedDate.Date)
                {
                    TimeSpan tourTime = tour.Date.TimeOfDay;
                    TimeSpan inputTime = TimeSpan.Parse(time);

                    if (tourTime == inputTime)
                    {
                        string timeOnly = tour.Date.ToString("HH:mm");
                        string status = tour.Status ? "Active" : "Inactive";

                        table.AddRow(
                            timeOnly,
                            tour.Language,
                            status
                        );
                    }
                }
            }

            AnsiConsole.Render(table);
        }
    }
}
