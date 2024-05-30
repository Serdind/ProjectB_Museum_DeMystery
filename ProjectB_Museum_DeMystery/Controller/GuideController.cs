public class GuideController
{
    private static IMuseum museum = Program.Museum;
    public void ViewVisitorsTour(int tourId, GuidedTour tour, Guide guide)
    {
        DateTime currentDate = museum.Today;
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "unique_codes.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        List<string> uniqueCodes = UniqueCodes.LoadUniqueCodesFromFile(filePath);

        if (tour.ID == tourId && tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= museum.Now.TimeOfDay && tour.Status)
        {
            Tour.OverviewVisitorsTour(tourId);

            while (true)
            {
                string option = GuideOptions.Options(tourId);

                if (option.ToLower() == "a" || option.ToLower() == "add visitor")
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
                            guide.AddVisistorToTour(tourId, qr);
                            codeValid = true;
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
                    break;
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
            string option = ViewVisitors.Show();
            int tourID;

            if (option.ToLower() == "v" || option.ToLower() == "view visitors")
            {
                AdminOptions.BackOption();
                tourID = TourId.WhichTourId();

                if (tourID == -1)
                {
                    break;
                }

                bool tourFound = false;
                foreach (var tour in tours)
                {
                    if (tour.ID == tourID)
                    {
                        ViewVisitorsTour(tourID, tour, guide);
                        tourFound = true;
                        break;
                    }
                }

                if (!tourFound)
                {
                    TourNotFound.Show();
                }
            }
            else if (option.ToLower() == "s" || option.ToLower() == "start tour")
            {
                AdminOptions.BackOption();
                tourID = TourId.WhichTourId();

                if (tourID == -1)
                {
                    break;
                }

                Tour.guide.StartTour(tourID);
                break;
            }
            else if (option.ToLower() == "b" || option.ToLower() == "go back")
            {
                break;
            }
            else
            {
                WrongInput.Show();
            }
        }
    }
}