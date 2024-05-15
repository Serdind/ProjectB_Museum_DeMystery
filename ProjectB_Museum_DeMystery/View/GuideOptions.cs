using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class GuideOptions : View
{
    public static string Options(int tourID)
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
                Console.WriteLine("Add visitor(A)\nRemove visitor(R)\nGo back(B)");
            }
            else
            {
                Console.WriteLine("Add visitor(A)\nGo back(B)");
            }
        }
            
        return ReadLineString();
    }

    public static void StartTour(GuidedTour tour)
    {
        string message = $"The tour has been started:" +
                        $"Tour: {tour.Name}\n" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Language: {tour.Language}\n";

        Console.WriteLine(message);
    }

    public static string ViewTours()
    {
        Console.WriteLine("My tours(M)\nDone(D)");
        return ReadLineString();
    }

    public static void AddedVisitorToTour()
    {
        Console.WriteLine("Succesfully added visitor to tour.");
    }

    public static void RemovedVisitorFromTour()
    {
        Console.WriteLine("Succesfully removed visitor from tour.");
    }

    public static void VisitorNotFound()
    {
        Console.WriteLine("Visitor not found in the visitors list.");
    }

    public static void VisitorNotFoundInTour()
    {
        Console.WriteLine("Visitor not found in the tour's list of reserved visitors.");
    }

    public static void TourNotFoundOrActive()
    {
        Console.WriteLine("Tour not found or not active.");
    }

    public static void TourVisitorFileNotFound()
    {
        Console.WriteLine("Tour not found or not active.");
    }
}
