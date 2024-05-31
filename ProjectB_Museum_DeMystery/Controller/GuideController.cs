using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;

public class GuideController
{
    private static IMuseum museum = Program.Museum;
    public void ViewVisitorsTour(int tourId, GuidedTour tour, Guide guide, List<GuidedTour> tours)
    {
        DateTime currentDate = museum.Today;
        string filePath = Model<UniqueCodes>.GetFileNameUniqueCodes();
        List<string> uniqueCodes = UniqueCodes.LoadUniqueCodesFromFile(filePath);

        if (tour.ID == tourId && tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= museum.Now.TimeOfDay && tour.Status)
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                Tour.OverviewVisitorsTour(tourId);
                string option = GuideOptions.Options(tourId);

                if (option.ToLower() == "a" || option.ToLower() == "add visitor")
                {
                    AdminOptions.BackOption();
                    while (true)
                    {
                        string qr = QRVisitor.WhichVisitorQr();

                        if (qr.ToLower() == "b" || qr.ToLower() == "back")
                        {
                            break;
                        }

                        if (uniqueCodes.Contains(qr))
                        {
                            guide.AddVisistorToTour(tourId, qr);
                        }
                        else
                        {
                            CodeNotValid.Show();
                        }
                    }
                }
                else if (option.ToLower() == "r" || option.ToLower() == "remove visitor")
                {
                    bool codeValid = false;

                    while (!codeValid)
                    {
                        AdminOptions.BackOption();
                        string qr = QRVisitor.WhichVisitorQr();

                        if (qr.ToLower() == "b" || qr.ToLower() == "back")
                        {
                            break;
                        }

                        if (uniqueCodes.Contains(qr))
                        {
                            guide.RemoveVisitorFromTour(tourId, qr);
                            codeValid = true;
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
                    OptionsGuide(tours, guide);
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
            bool tourFound = false;

            string option = ViewVisitors.Show();

            if (option.ToLower() == "v" || option.ToLower() == "view visitors")
            {
                AdminOptions.BackOption();
                while (!tourFound)
                {
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
                            tourFound = true;
                            break;
                        }
                    }

                    if (!tourFound)
                    {
                        TourNotFound.Show();
                    }
                }

                if (tourFound)
                {
                    break;
                }
            }
            else if (option.ToLower() == "s" || option.ToLower() == "start tour")
            {
                AdminOptions.BackOption();
                tourID = TourId.WhichTourId();

                if (tourID == -1)
                {
                    continue;
                }

                Tour.guide.StartTour(tourID);
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
        DateTime today = DateTime.Today;
        string filePath = Model<GuidedTour>.GetFileNameTours();

        if (museum.FileExists(filePath))
        {
            string jsonData = museum.ReadAllText(filePath);
            List<GuidedTour> toursFile = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);

            List<GuidedTour> guideTours = toursFile.FindAll(tour => tour.NameGuide == guide.Name && tour.Date.Date == today);

            var table = new Table().LeftAligned();
            table.AddColumn("ID");
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
                        dateOnly,
                        timeOnly,
                        GuidedTour.StartingPoint,
                        GuidedTour.EndPoint,
                        tour.Language,
                        remainingSpots.ToString()
                    );
                }
            }

            AnsiConsole.Render(table);
        }
    }
}