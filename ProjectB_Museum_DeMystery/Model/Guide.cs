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
                GuideOptions.AddedVisitorToTour();
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

    public bool RemoveVisitorFromTour(int tourID, string qr)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string toursFileName = "tours.json";
        string visitorsFileName = "visitors.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string toursFilePath = Path.Combine(userDirectory, subdirectory, toursFileName);
        string visitorsFilePath = Path.Combine(userDirectory, subdirectory, visitorsFileName);

        if (File.Exists(toursFilePath) && File.Exists(visitorsFilePath))
        {
            string toursJson = File.ReadAllText(toursFilePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(toursJson);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Status == true)
            {
                var visitorToRemove = tour.ReservedVisitors.FirstOrDefault(visitor => visitor.QR == qr);

                if (visitorToRemove != null)
                {
                    tour.ReservedVisitors.Remove(visitorToRemove);

                    Tour.SaveToursToFile(toursFilePath, tours);

                    string visitorsJson = File.ReadAllText(visitorsFilePath);
                    var visitors = JsonConvert.DeserializeObject<List<Visitor>>(visitorsJson);
                    var visitorInAllVisitors = visitors.FirstOrDefault(visitor => visitor.QR == qr);

                    if (visitorInAllVisitors != null)
                    {
                        visitors.Remove(visitorInAllVisitors);
                        File.WriteAllText(visitorsFilePath, JsonConvert.SerializeObject(visitors));
                        GuideOptions.RemovedVisitorFromTour();
                    }
                    else
                    {
                        GuideOptions.VisitorNotFound();
                    }

                    return true;
                }
                else
                {
                    GuideOptions.VisitorNotFoundInTour();
                    return false;
                }
            }
            else
            {
                GuideOptions.TourNotFoundOrActive();
                return false;
            }
        }
        else
        {
            GuideOptions.TourVisitorFileNotFound();
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
                        int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                        
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
                TourNotFound.Show();
            }
        }
        else
        {
            TourNotAvailable.Show();
        }
    }
}
