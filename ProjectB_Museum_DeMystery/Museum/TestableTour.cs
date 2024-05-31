using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;

public class TestableTour
{
    public readonly IMuseum Museum;

    public TestableTour(IMuseum museum)
    {
        Museum = museum;
    }

    public readonly List<GuidedTour> guidedTour = new List<GuidedTour>();
    public readonly List<DepartmentHead> admins = new List<DepartmentHead>();
    public readonly List<Guide> guides = new List<Guide>();
    public List<Visitor> visitors = new List<Visitor>();
    public Guide guide = new Guide("Casper", "4892579");

    public void UpdateTours()
    {
        DateTime today = DateTime.Today;
        DateTime endDate = today.AddDays(1);
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
            
        if (Museum.FileExists(filePath))
        {
            DateTime lastWriteTime = Museum.GetLastWriteTime(filePath).Date;
            
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

    public void ToursDay(DateTime startDate, DateTime endDate)
    {
        DateTime currentDate = startDate.Date;
        DateTime today = Museum.Now.Date;

        while (currentDate <= endDate.Date)
        {
            if (currentDate.Date >= today && currentDate.Date <= endDate.Date)
            {
                AddTour(new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 11, 30, 0), "English", guide.Name));
                AddTour(new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 00, 0), "Dutch", guide.Name));
                AddTour(new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 30, 0), "English", guide.Name));
                AddTour(new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 14, 00, 0), "Dutch", guide.Name));
                AddTour(new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 14, 30, 0), "English", guide.Name));
                AddTour(new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 15, 00, 0), "Dutch", guide.Name));
                AddTour(new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 16, 00, 0), "English", guide.Name));
                AddTour(new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 16, 30, 0), "Dutch", guide.Name));
                AddTour(new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 17, 00, 0), "English", guide.Name));
            }
            currentDate = currentDate.AddDays(1);
        }

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        SaveToursToFile(filePath, guidedTour);
    }


    public void AddTour(GuidedTour tour)
    {
        List<GuidedTour> tours = LoadToursFromFile();
        
        int newID = tours.Count > 0 ? tours.Max(t => t.ID) + 1 : 1;
        
        tour.ID = newID;
        tours.Add(tour);

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        
        SaveToursToFile(filePath, tours);
    }

    public void SaveToursToFile(string filePath, List<GuidedTour> tours)
    {
        List<GuidedTour> existingTours;
        if (Museum.FileExists(filePath))
        {
            string existingJson = Museum.ReadAllText(filePath);
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
        Museum.WriteAllText(filePath, updatedJson);
    }

    public bool OverviewTours(bool edit)
    {
        DateTime currentDate = DateTime.Today;
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (edit)
        {
            if (Museum.FileExists(filePath))
            {
                string json = Museum.ReadAllText(filePath);
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

                Museum.WriteLine(table.ToString());
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
            if (Museum.FileExists(filePath))
            {
                string json = Museum.ReadAllText(filePath);
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

                bool anyToursToday = false;

                foreach (var tour in tours)
                {
                    if (tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= DateTime.Now.TimeOfDay && tour.Status)
                    {
                        anyToursToday = true;
                        string timeOnly = tour.Date.ToString("HH:mm");
                        string dateOnly = tour.Date.ToShortDateString();
                        int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;

                        table.AddRow(
                            tour.ID.ToString(),
                            dateOnly,
                            timeOnly,
                            "40 minutes",
                            tour.Language,
                            guide.Name,
                            remainingSpots.ToString()
                        );
                    }
                }

                if (anyToursToday)
                {
                    Museum.WriteLine(table.ToString());
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

    public void RemoveToursFromDate(DateTime date)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        
        if (Museum.FileExists(filePath))
        {
            string json = Museum.ReadAllText(filePath);
            List<GuidedTour> tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            tours.RemoveAll(tour => tour.Date.Date == date.Date);

            string updatedJson = JsonConvert.SerializeObject(tours, Formatting.Indented);
            Museum.WriteAllText(filePath, updatedJson);
        }
    }

    public List<GuidedTour> LoadToursFromFile()
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

    public void AddAdminToJSON()
    {
        AddAdmin(new DepartmentHead("Frans", "6457823"));

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "adminsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        SaveAdminToFile(filePath);
    }

    public void AddAdmin(DepartmentHead departmentHead)
    {
        admins.Add(departmentHead);
    }

    public void SaveAdminToFile(string filePath)
    {
        string json = JsonConvert.SerializeObject(admins, Formatting.Indented);

        Museum.WriteAllText(filePath, json);
    }

    public List<DepartmentHead> LoadAdminsFromFile()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "adminsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (Museum.FileExists(filePath))
        {
            string json = Museum.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<DepartmentHead>>(json);
        }
        else
        {
            return new List<DepartmentHead>();
        }
    }

    public void AddGuideToJSON()
    {
        AddGuide(new Guide("Casper", "4892579"));

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "guidesTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        SaveGuideToFile(filePath);
    }

    public void AddGuide(Guide guide)
    {
        guides.Add(guide);
    }

    public void SaveGuideToFile(string filePath)
    {
        string json = JsonConvert.SerializeObject(guides, Formatting.Indented);

        Museum.WriteAllText(filePath, json);
    }

    public List<Guide> LoadGuidesFromFile()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "guidesTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (Museum.FileExists(filePath))
        {
            string json = Museum.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Guide>>(json);
        }
        else
        {
            return new List<Guide>();
        }
    }
    
    public void AddVisitorToJSON(int tourId, string qr)
    {
        List<Visitor> existingVisitors = LoadVisitorsFromFile();

        Visitor newVisitor = new Visitor(tourId, qr);

        int nextId = existingVisitors.Count > 0 ? existingVisitors.Max(v => v.Id) + 1 : 1;

        newVisitor.Id = nextId;

        existingVisitors.Add(newVisitor);

        SaveVisitorToFile(existingVisitors);
    }

    public List<Visitor> LoadVisitorsFromFile()
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

    public void AddVisitor(Visitor visitor)
    {
        visitors.Add(visitor);
    }

    public void SaveVisitorToFile(List<Visitor> visitors)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string json = JsonConvert.SerializeObject(visitors, Formatting.Indented);
        Museum.WriteAllText(filePath, json);
    }

    public void OverviewVisitorsTour(int tourID)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (Museum.FileExists(filePath))
        {
            string json = Museum.ReadAllText(filePath);
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

                Museum.WriteLine(table.ToString());
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

    public void CreateEmptyJsonFile(string filePath)
    {
        if (!Museum.FileExists(filePath))
        {
            Museum.WriteAllText(filePath, "[]");
        }
    }
}