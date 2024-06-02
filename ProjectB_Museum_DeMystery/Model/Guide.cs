using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;

public class Guide : Person
{
    private static IMuseum museum = Program.Museum;
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

    public bool AddVisitorToTour(int tourID, string qr)
    {
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Status == true)
            {
                Visitor visitor = new Visitor(tourID, qr);
                
                if (visitor.ReservateByGuide(tourID, visitor))
                {
                    GuideOptions.AddedVisitorToTour();
                    return true;
                }
                else
                {
                    MaxReservation.GuideShow();
                    return false;
                }
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
        string toursFilePath = Model<GuidedTour>.GetFileNameTours();
        string visitorsFilePath = Model<Visitor>.GetFileNameVisitors();

        if (museum.FileExists(toursFilePath) && museum.FileExists(visitorsFilePath))
        {
            string toursJson = museum.ReadAllText(toursFilePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(toursJson);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null && tour.Status == true)
            {
                var visitorToRemove = tour.ReservedVisitors.FirstOrDefault(visitor => visitor.QR == qr);

                if (visitorToRemove != null)
                {
                    tour.ReservedVisitors.Remove(visitorToRemove);

                    Tour.SaveToursToFile(toursFilePath, tours);

                    string visitorsJson = museum.ReadAllText(visitorsFilePath);
                    var visitors = JsonConvert.DeserializeObject<List<Visitor>>(visitorsJson);
                    var visitorInAllVisitors = visitors.FirstOrDefault(visitor => visitor.QR == qr);

                    if (visitorInAllVisitors != null)
                    {
                        visitors.Remove(visitorInAllVisitors);
                        museum.WriteAllText(visitorsFilePath, JsonConvert.SerializeObject(visitors));
                        GuideOptions.RemovedVisitorFromTour();
                        return true;
                    }
                    else
                    {
                        GuideOptions.VisitorNotFound();
                        return false;
                    }
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

    public bool ViewTours(string guideName)
    {
        DateTime today = DateTime.Today;
        string filePath = Model<GuidedTour>.GetFileNameTours();
        bool toursFound = false;

        if (museum.FileExists(filePath))
        {
            string jsonData = museum.ReadAllText(filePath);
            List<GuidedTour> tours = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);

            List<GuidedTour> guideTours = tours.FindAll(tour => tour.NameGuide == guideName && tour.Date.Date == today);

            if (guideTours.Any())
            {
                GuideController guideController = new GuideController();
                guideController.OptionsGuide(guideTours, Tour.guide);
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

    public void StartTour(int tourID)
    {
        DateTime currentDate = DateTime.Now;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string json = museum.ReadAllText(filePath);
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
