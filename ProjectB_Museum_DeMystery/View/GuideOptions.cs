using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class GuideOptions : View
{
    

    public static string Options(int tourID)
    {
        IMuseum museum = Program.Museum;
        string filePath = Model<Visitor>.GetFileNameVisitors();
        
        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json);
            
            visitors = visitors.Where(v => v.TourId == tourID).OrderBy(t => t.TourId).ToList();

            string filePath1 = Model<GuidedTour>.GetFileNameTours();
            string json1 = museum.ReadAllText(filePath1);
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
                        museum.WriteLine("");
                        museum.WriteLine("Add visitor(A)\nRemove visitor(R)\nGo back(B)");
                    }
                    else
                    {
                        museum.WriteLine("");
                        museum.WriteLine("Remove visitor(R)\nGo back(B)");
                    }
                }
                else
                {
                    museum.WriteLine("");
                    museum.WriteLine("Add visitor(A)\nGo back(B)");
                }
            }
        }
            
        return ReadLineString();
    }

    public static void StartTour(GuidedTour tour)
    {
        IMuseum museum = Program.Museum;
        string message = $"The tour has been started:" +
                        $"Date: {tour.Date.ToShortDateString()}\n" +
                        $"Time: {tour.Date.ToString("HH:mm")}\n" +
                        $"Duration: 40 minutes\n" +
                        $"Language: {tour.Language}\n" +
                        $"Guide: {tour.NameGuide}\n";
    
        museum.WriteLine("");
        museum.WriteLine(message);
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static string ViewTours()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("My tour(M)\nLog out(L)");
        return ReadLineString();
    }

    public static void AddedVisitorToTour()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Succesfully added visitor to tour.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void RemovedVisitorFromTour()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Succesfully removed visitor from tour.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void VisitorNotFound()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Visitor not found in the visitors list.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
    }

    public static void VisitorNotFoundInTour()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Visitor not found in the tour's list of reserved visitors.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void TourNotFoundOrActive()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Tour not found or not active.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void TourVisitorFileNotFound()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Tour not found or not active.");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static string GuideName()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Insert the name of guide:");
        return ReadLineString();
    }
}