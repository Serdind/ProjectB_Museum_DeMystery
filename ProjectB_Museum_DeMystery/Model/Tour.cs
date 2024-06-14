using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;

public static class Tour
{
    public static void UpdateTours()
    {
        IMuseum museum = Program.Museum;
        ClearOldVisitors();

        string filePath = Model<GuidedTour>.GetFileNameTours();

        DateTime today = DateTime.Today;
        DateTime tomorrow = today.AddDays(1);

        if (museum.FileExists(filePath))
        {
            List<GuidedTour> existingTours = LoadToursFromFile();

            existingTours = existingTours.Where(tour => tour.Date.Date == today || tour.Date.Date == tomorrow).ToList();

            List<GuidedTour> toursToday = existingTours.Where(tour => tour.Date.Date == today).ToList();

            List<GuidedTour> toursTomorrow = toursToday.Select(tour =>
            {
                return new GuidedTour(
                    new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, tour.Date.Hour, tour.Date.Minute, tour.Date.Second),
                    tour.Language,
                    tour.NameGuide
                );
            }).ToList();

            List<GuidedTour> updatedTours = toursToday.Concat(toursTomorrow).ToList();

            for (int i = 0; i < updatedTours.Count; i++)
            {
                updatedTours[i].ID = i + 1;
            }

            SaveToursToFile(filePath, updatedTours);
        }
        else
        {
            List<GuidedTour> defaultToursToday = GenerateDefaultToursForDay(today).ToList();
            List<GuidedTour> defaultToursTomorrow = defaultToursToday.Select(tour =>
            {
                return new GuidedTour(
                    new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, tour.Date.Hour, tour.Date.Minute, tour.Date.Second),
                    tour.Language,
                    tour.NameGuide
                );
            }).ToList();

            List<GuidedTour> defaultTours = defaultToursToday.Concat(defaultToursTomorrow).ToList();

            for (int i = 0; i < defaultTours.Count; i++)
            {
                defaultTours[i].ID = i + 1;
            }

            SaveToursToFile(filePath, defaultTours);
        }
    }

    public static List<GuidedTour> GenerateDefaultToursForDay(DateTime date)
    {
        return new List<GuidedTour>
        {
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 10, 40, 0), "English", "Casper"),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 11, 20, 0), "Dutch", "Bas"),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 12, 00, 0), "English", "Rick"),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 13, 40, 0), "Dutch", "Casper"),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 14, 00, 0), "English", "Bas"),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 15, 40, 0), "Dutch", "Rick"),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 16, 20, 0), "English", "Casper"),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 16, 00, 0), "Dutch", "Bas"),
            new GuidedTour(new DateTime(date.Year, date.Month, date.Day, 17, 00, 0), "English", "Rick")
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
        IMuseum museum = Program.Museum;
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
        IMuseum museum = Program.Museum;
        DateTime currentDate = DateTime.Today;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (edit)
        {
            
            string selection;

            while (true)
            {
                selection = AdminOptions.SelectTours();

                if (selection.ToLower() == "t" || selection.ToLower() == "tours from today" ||
                    selection.ToLower() == "a" || selection.ToLower() == "tours from tomorrow" ||
                    selection.ToLower() == "b" || selection.ToLower() == "back")
                {
                    break;
                }
                else
                {
                    WrongInput.Show();
                }
            }

            if (selection.ToLower() == "t" || selection.ToLower() == "tours from today")
            {
                if (museum.FileExists(filePath))
                {
                    string json = museum.ReadAllText(filePath);
                    var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

                    tours = tours.OrderBy(t => t.Date).ToList();

                    museum.WriteLine("+-----------+------------+----------+------------+---------+--------------+----------------+--------+");
                    museum.WriteLine("| Tour ID   | Date       | Time     | Duration   | Language| Guide        | Remaining Spots| Status |");
                    museum.WriteLine("+-----------+------------+----------+------------+---------+--------------+----------------+--------+");

                    foreach (var tour in tours)
                    {
                        if (tour.Date.Date == currentDate)
                        {
                            string timeOnly = tour.Date.ToString("HH:mm");
                            string dateOnly = tour.Date.ToShortDateString();
                            int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                            string status = tour.Status ? "Active" : "Inactive";

                            string tourInfo = $"| {tour.ID,-9} | {dateOnly,-10} | {timeOnly,-8} | 40 minutes  | {tour.Language,-7} | {tour.NameGuide,-12} | {remainingSpots,-14} | {status,-6} |";

                            museum.WriteLine(tourInfo);
                        }
                    }

                    museum.WriteLine("+-----------+------------+----------+------------+---------+--------------+----------------+--------+");

                    return true;
                }
                else
                {
                    TourEmpty.Show();
                    return false;
                }
            }
            else if (selection.ToLower() == "a" || selection.ToLower() == "tours from tomorrow")
            {
                OverviewToursTomorrow();
                return true;
            }
            else if (selection.ToLower() == "b" || selection.ToLower() == "back")
            {
                return false;
            }
            return false;
        }
        else
        {
            if (museum.FileExists(filePath))
            {
                string json = museum.ReadAllText(filePath);
                var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);
                tours = tours.OrderBy(t => t.Date).ToList();

                bool anyToursToday = false;
                
                museum.WriteLine("+-----------+------------+----------+------------+---------+--------------+----------------+");
                museum.WriteLine("| ID        | Date       | Time     | Duration   | Language| Guide        | Remaining spots|");
                museum.WriteLine("+-----------+------------+----------+------------+---------+--------------+----------------+");
                
                foreach (var tour in tours)
                {
                    if (tour.Date.Date == museum.Today.Date && tour.Date.TimeOfDay >= DateTime.Now.TimeOfDay && tour.Status)
                    {
                        anyToursToday = true;
                        string timeOnly = tour.Date.ToString("HH:mm");
                        string dateOnly = tour.Date.ToShortDateString();
                        int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;

                        museum.WriteLine($"| {tour.ID,-9} | {dateOnly,-10} | {timeOnly,-8} | 40 minutes  | {tour.Language,-7} | {tour.NameGuide,-12} | {remainingSpots,-14} |");
                    }
                }
                
                museum.WriteLine("+-----------+------------+----------+------------+---------+--------------+----------------+");

                if (anyToursToday)
                {
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
                return false;
            }
        }
    }

    public static bool OverviewToursTomorrow()
    {
        IMuseum museum = Program.Museum;
        DateTime currentDate = DateTime.Today;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        DateTime tomorrowDate = currentDate.AddDays(1);

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            tours = tours.OrderBy(t => t.Date).ToList();

            museum.WriteLine("+------------+----------+------------+---------+--------------+--------+");
            museum.WriteLine("| Date       | Time     | Duration   | Language| Guide        | Status |");
            museum.WriteLine("+------------+----------+------------+---------+--------------+--------+");

            foreach (var tour in tours)
            {
                if (tour.Date.Date == tomorrowDate.Date)
                {
                    string timeOnly = tour.Date.ToString("HH:mm");
                    string dateOnly = tour.Date.ToShortDateString();
                    int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                    string status = tour.Status ? "Active" : "Inactive";

                    museum.WriteLine($"| {dateOnly,-10} | {timeOnly,-8} | 40 minutes  | {tour.Language,-7} | {tour.NameGuide,-12} | {status,-6} |");
                }
            }

            museum.WriteLine("+------------+----------+------------+---------+--------------+--------+");
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
        IMuseum museum = Program.Museum;
        DateTime tomorrowDate = museum.Today.AddDays(1);
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tomorrowTours = tours.Where(t => t.Date.Date == tomorrowDate.Date).OrderBy(t => t.Date).ToList();

            if (tomorrowTours.Any())
            {
                museum.WriteLine("+----------+------------+------------+------------+--------+");
                museum.WriteLine("| Time     | Duration   | Language   | Guide      | Status |");
                museum.WriteLine("+----------+------------+------------+------------+--------+");

                foreach (var tour in tomorrowTours)
                {
                    string timeOnly = tour.Date.ToString("HH:mm");
                    int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                    string status = tour.Status ? "Active" : "Inactive";

                    museum.WriteLine($"| {timeOnly,-8} | 40 minutes | {tour.Language,-10} | {tour.NameGuide,-10} | {status,-6} |");
                }

                museum.WriteLine("+----------+------------+------------+------------+--------+");
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
        IMuseum museum = Program.Museum;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GuidedTour>>(json);
        }

        return new List<GuidedTour>();
    }

    public static void AddAdminToJSON(List<DepartmentHead> admins)
    {
        string filePath = Model<DepartmentHead>.GetFileNameAdmins();
        SaveAdminToFile(filePath,admins);
    }

    public static void AddAdmin(DepartmentHead departmentHead, List<DepartmentHead> admins)
    {
        admins.Add(departmentHead);
    }

    public static void SaveAdminToFile(string filePath, List<DepartmentHead> admins)
    {
        IMuseum museum = Program.Museum;
        string json = JsonConvert.SerializeObject(admins, Formatting.Indented);

        museum.WriteAllText(filePath, json);
    }

    public static List<DepartmentHead> LoadAdminsFromFile()
    {
        IMuseum museum = Program.Museum;
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

    public static void AddGuideToJSON(List<Guide> guides)
    {
        string filePath = Model<Guide>.GetFileNameGuides();
        SaveGuideToFile(filePath, guides);
    }

    public static void AddGuide(Guide guide, List<Guide> guides)
    {
        guides.Add(guide);
    }

    public static void SaveGuideToFile(string filePath, List<Guide> guides)
    {
        IMuseum museum = Program.Museum;
        string json = JsonConvert.SerializeObject(guides, Formatting.Indented);

        museum.WriteAllText(filePath, json);
    }

    public static List<Guide> LoadGuidesFromFile()
    {
        IMuseum museum = Program.Museum;
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
        IMuseum museum = Program.Museum;
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

    public static void AddVisitor(Visitor visitor, List<Visitor> visitors)
    {
        visitors.Add(visitor);
    }

    public static void SaveVisitorToFile(List<Visitor> visitors)
    {
        IMuseum museum = Program.Museum;
        string filePath = Model<Visitor>.GetFileNameVisitors();

        string json = JsonConvert.SerializeObject(visitors, Formatting.Indented);
        museum.WriteAllText(filePath, json);
    }

    public static bool OverviewVisitorsTour(int tourID)
    {
        IMuseum museum = Program.Museum;
        string filePath = Model<Visitor>.GetFileNameVisitors();
        
        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json);
            
            visitors = visitors.Where(v => v.TourId == tourID).OrderBy(t => t.TourId).ToList();

            if (visitors.Any())
            {
                museum.WriteLine("+-----+---------------------+");
                museum.WriteLine("| ID  | QR                  |");
                museum.WriteLine("+-----+---------------------+");

                foreach (var visitor in visitors)
                {
                    museum.WriteLine($"| {visitor.Id,-4} | {visitor.QR,-20} |");
                }

                museum.WriteLine("+-----+---------------------+");
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
        IMuseum museum = Program.Museum;
        if (!museum.FileExists(filePath))
        {
            museum.WriteAllText(filePath, "[]");
        }
    }

    public static void ClearOldVisitors()
    {
        IMuseum museum = Program.Museum;
        string filePath = Model<Visitor>.GetFileNameVisitors();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json);

            var today = DateTime.Today;
            visitors = visitors.Where(v => v.dateAdded.Date == today).ToList();

            SaveVisitorToFile(visitors);
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
        IMuseum museum = Program.Museum;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string jsonData = museum.ReadAllText(filePath);
            List<GuidedTour> toursFile = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);

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

                        museum.WriteLine("+--------+------------+--------+");
                        museum.WriteLine("| Time   | Language   | Status |");
                        museum.WriteLine("+--------+------------+--------+");
                        museum.WriteLine($"| {timeOnly,-7} | {tour.Language,-10} | {status,-6} |");
                        museum.WriteLine("+--------+------------+--------+");
                    }
                }
            }
        }
    }
}
