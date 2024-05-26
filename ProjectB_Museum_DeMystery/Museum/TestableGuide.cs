using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using Microsoft.VisualBasic;

public class TestableGuide
{
    public readonly IMuseum Museum;

    public TestableGuide(IMuseum museum)
    {
        Museum = museum;
    }
    public bool AddVisistorToTour(int tourID, string qr)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (Museum.FileExists(filePath))
        {
            string json = Museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Status == true)
            {
                Visitor visitor = new Visitor(tourID, qr);
                TestableVisitor testVisitor = new TestableVisitor(Museum);
                
                testVisitor.ReservateByGuide(tourID, visitor);

                Museum.WriteLine("Succesfully added visitor to tour.");
                return true;
            }
            else
            {
                Museum.WriteLine("Tour not found.");
                return false;
            }
        }
        return false;
    }

    public bool RemoveVisitorFromTour(int tourID, string qr)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string toursFileName = "toursTest.json";
        string visitorsFileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string toursFilePath = Path.Combine(userDirectory, subdirectory, toursFileName);
        string visitorsFilePath = Path.Combine(userDirectory, subdirectory, visitorsFileName);

        TestableTour testableTour = new TestableTour(Museum);

        if (Museum.FileExists(toursFilePath) && Museum.FileExists(visitorsFilePath))
        {
            string toursJson = Museum.ReadAllText(toursFilePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(toursJson);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Status == true)
            {
                var visitorToRemove = tour.ReservedVisitors.FirstOrDefault(visitor => visitor.QR == qr);

                if (visitorToRemove != null)
                {
                    tour.ReservedVisitors.Remove(visitorToRemove);

                    testableTour.SaveToursToFile(toursFilePath, tours);

                    string visitorsJson = Museum.ReadAllText(visitorsFilePath);
                    var visitors = JsonConvert.DeserializeObject<List<Visitor>>(visitorsJson);
                    var visitorInAllVisitors = visitors.FirstOrDefault(visitor => visitor.QR == qr);

                    if (visitorInAllVisitors != null)
                    {
                        visitors.Remove(visitorInAllVisitors);
                        Museum.WriteAllText(visitorsFilePath, JsonConvert.SerializeObject(visitors));

                        Museum.WriteLine("Succesfully removed visitor from tour.");
                        return true;
                    }
                    else
                    {
                        Museum.WriteLine("Visitor not found in the visitors list.");
                        return false;
                    }
                }
                else
                {
                    Museum.WriteLine("Visitor not found in the tour's list of reserved visitors.");
                    return false;
                }
            }
            else
            {
                Museum.WriteLine("Tour not found or not active.");
                return false;
            }
        }
        else
        {
            Museum.WriteLine("Files not found.");
            return false;
        }
    }

    public bool ViewTours(string guideName)
    {
        DateTime today = DateTime.Today;
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        string jsonData = Museum.ReadAllText(filePath);
        
        if (Museum.FileExists(filePath))
        {
            List<GuidedTour> tours = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);
            
            List<GuidedTour> guideTours = tours.FindAll(tour => tour.NameGuide == guideName);
            
            var table = new Table().LeftAligned();
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
                if (tour.Status)
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
                    
                    Museum.WriteLine(table.ToString());
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    public void StartTour(int tourID)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (Museum.FileExists(filePath))
        {
            string json = Museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null)
            {
                string message = $"Tour has been started:\n" +
                    $"Tour: {tour.Name}\n" +
                    $"Date: {tour.Date.ToShortDateString()}\n" +
                    $"Time: {tour.Date.ToString("HH:mm")}\n" +
                    $"Language: {tour.Language}\n";

                Museum.WriteLine(message);
            }
            else
            {
                Museum.WriteLine("Tour not found.");
            }
        }
        else
        {
            Museum.WriteLine("Tour is not available.");
        }
    }
}