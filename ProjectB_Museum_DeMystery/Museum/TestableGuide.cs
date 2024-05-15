using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;

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
                
                visitor.Reservate(tourID, visitor);
                visitor.Reservate(tourID, visitor);

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