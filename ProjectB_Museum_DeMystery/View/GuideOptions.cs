using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class GuideOptions : View
{
    private static IMuseum museum = Program.Museum;

    public static string Options(int tourID)
    {
        string filePath = Model<Visitor>.GetFileNameVisitors();
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json);
            
            visitors = visitors.Where(v => v.TourId == tourID).OrderBy(t => t.TourId).ToList();

            string filePath1 = Model<GuidedTour>.GetFileNameTours();
            string json1 = File.ReadAllText(filePath1);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json1);

            var tour = tours.FirstOrDefault(v => v.ID == tourID);

            if (tour != null)
            {
                bool isTourFull = tour.ReservedVisitors.Count >= tour.MaxParticipants;

                if (visitors.Any())
                {
                    Tour.OverviewVisitorsTour(tourID);
                    if (!isTourFull)
                    {
                        museum.WriteLine("Add visitor(A)\nRemove visitor(R)\nGo back(B)");
                    }
                    else
                    {
                        museum.WriteLine("Remove visitor(R)\nGo back(B)");
                    }
                }
                else
                {
                    museum.WriteLine("Add visitor(A)\nGo back(B)");
                }
            }
        }
            
        return ReadLineString();
    }

    public static void StartTour(GuidedTour tour)
    {
        string message = $"The tour has been started:" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 40 minutes\n" +
                        $"Language: {tour.Language}\n" +
                        $"Guide: {tour.NameGuide}\n";

        museum.WriteLine(message);
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static string ViewTours()
    {
        museum.WriteLine("My tour(M)\nLog out(L)");
        return ReadLineString();
    }

    public static void AddedVisitorToTour()
    {
        museum.WriteLine("Succesfully added visitor to tour.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void RemovedVisitorFromTour()
    {
        museum.WriteLine("Succesfully removed visitor from tour.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void VisitorNotFound()
    {
        museum.WriteLine("Visitor not found in the visitors list.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void VisitorNotFoundInTour()
    {
        museum.WriteLine("Visitor not found in the tour's list of reserved visitors.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static void TourNotFoundOrActive()
    {
        museum.WriteLine("Tour not found or not active.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static void TourVisitorFileNotFound()
    {
        museum.WriteLine("Tour not found or not active.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static string GuideName()
    {
        museum.WriteLine("Insert the name of guide:");
        return ReadLineString();
    }
}