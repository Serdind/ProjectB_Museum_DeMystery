using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class GuideController
{
    
    public void ViewVisitorsTour(int tourId, GuidedTour tour, Guide guide, List<GuidedTour> tours)
    {
        DateTime currentDate = DateTime.Today;
        string filePath = Model<UniqueCodes>.GetFileNameUniqueCodes();
        List<string> uniqueCodes = UniqueCodes.LoadUniqueCodesFromFile(filePath);

        if (tour.ID == tourId && tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= DateTime.Now.TimeOfDay && tour.Status)
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                
                string option = GuideOptions.Options(tourId);

                if (option.ToLower() == "a" || option.ToLower() == "add visitor")
                {
                    
                    while (true)
                    {
                        AdminOptions.BackOption();
                        Tour.OverviewVisitorsTour(tourId);
                        string qr = QRVisitor.WhichVisitorQr();

                        if (qr.ToLower() == "b" || qr.ToLower() == "back")
                        {
                            break;
                        }

                        if (uniqueCodes.Contains(qr))
                        {
                            guide.AddVisitorToTour(tourId, qr);
                        }
                        else
                        {
                            CodeNotValid.Show();
                        }
                    }
                }
                else if (option.ToLower() == "r" || option.ToLower() == "remove visitor")
                {
                    

                    while (true)
                    {
                        AdminOptions.BackOption();
                        Tour.OverviewVisitorsTour(tourId);
                        string qr = QRVisitor.WhichVisitorQr();

                        if (qr.ToLower() == "b" || qr.ToLower() == "back")
                        {
                            break;
                        }

                        if (uniqueCodes.Contains(qr))
                        {
                            guide.RemoveVisitorFromTour(tourId, qr);
                        }
                        else
                        {
                            CodeNotValid.Show();
                        }
                    }
                }
                else if (option.ToLower() == "b" || option.ToLower() == "go back")
                {
                    keepRunning = false;
                }
                else
                {
                    WrongInput.Show();
                }
            }
        }
        else
        {
            
            TourNotAvailable.Show();
            
        }
    }

    public void OptionsGuide(List<GuidedTour> tours, Guide guide)
    {
        while (true)
        {
            int tourID;

            string option = ViewVisitors.Show();

            if (option.ToLower() == "v" || option.ToLower() == "view visitors")
            {
                while (true)
                {
                    
                    AdminOptions.BackOption();
                    ShowTableTours(guide);
                    tourID = TourId.WhichTourId();

                    if (tourID == -1)
                    {
                        break;
                    }

                    foreach (var tour in tours)
                    {
                        if (tour.ID == tourID)
                        {
                            ViewVisitorsTour(tourID, tour, guide, tours);
                        }
                    }
                }
            }
            else if (option.ToLower() == "s" || option.ToLower() == "start tour")
            {
                while (true)
                {
                    
                    AdminOptions.BackOption();
                    ShowTableTours(guide);
                    tourID = TourId.WhichTourId();

                    if (tourID == -1)
                    {
                        break;
                    }

                    var tour = tours.FirstOrDefault(t => t.ID == tourID);
                    if (tour != null)
                    {
                        guide.StartTour(tourID);
                    }
                    else
                    {
                        TourNotFound.Show();
                        continue;
                    }
                }
            }
            else if (option.ToLower() == "l" || option.ToLower() == "log out")
            {
                break;
            }
            else
            {
                WrongInput.Show();
            }
        }
    }

    public void ShowTableTours(Guide guide)
    {
        IMuseum museum = Program.Museum;
        DateTime today = DateTime.Today;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string jsonData = museum.ReadAllText(filePath);
            List<GuidedTour> toursFile = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);

            List<GuidedTour> guideTours = toursFile.FindAll(tour => tour.NameGuide == guide.Name && tour.Date.Date == today);

            if (guideTours.Any())
            {
                museum.WriteLine("+-----+------------+--------+----------------+----------+---------+----------------+");
                museum.WriteLine("| ID  | Date       | Time   | Starting Point | End Point| Language| Remaining spots|");
                museum.WriteLine("+-----+------------+--------+----------------+----------+---------+----------------+");

                foreach (var tour in guideTours)
                {
                    if (tour.Date.Date == today.Date && tour.Date.TimeOfDay >= DateTime.Now.TimeOfDay && tour.Status)
                    {
                        string timeOnly = tour.Date.ToString("HH:mm");
                        string dateOnly = tour.Date.ToShortDateString();
                        int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;

                        museum.WriteLine($"| {tour.ID,-4} | {dateOnly,-10} | {timeOnly,-6} | {GuidedTour.StartingPoint,-14} | {GuidedTour.EndPoint,-9} | {tour.Language,-7} | {remainingSpots,-14} |");
                    }
                }

                museum.WriteLine("+-----+------------+--------+----------------+----------+---------+----------------+");
            }
            else
            {
                TourInfo.NoToursToday();
            }
        }
        else
        {
            TourEmpty.Show();
        }
    }
}