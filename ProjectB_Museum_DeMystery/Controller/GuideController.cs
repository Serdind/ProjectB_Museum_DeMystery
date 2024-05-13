public class GuideController
{
    public void ViewVisitorsTour(int tourId, GuidedTour tour, Guide guide)
    {
        DateTime currentDate = DateTime.Today;

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "unique_codes.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        List<string> uniqueCodes = UniqueCodes.LoadUniqueCodesFromFile(filePath);

        if (tour.ID == tourId && tour.Date.Date == currentDate.Date && tour.Date.TimeOfDay >= DateTime.Now.TimeOfDay && tour.Status)
        {
            Tour.OverviewVisitorsTour(tourId);

            string option = GuideOptions.Options(tourId);

            string qr = QRVisitor.WhichVisitorQr();
            if (uniqueCodes.Contains(qr))
            {
                if (option.ToLower() == "a")
                {   
                    guide.AddVisistorToTour(tourId, qr);
                }
                else if (option.ToLower() == "r")
                {
                    guide.RemoveVisitorFromTour(tourId, qr);
                }
                else
                {
                    WrongInput.Show();
                }
            }
            else
            {
                Console.WriteLine("Code is not valid.");
            }
        }
        else
        {
            TourNotAvailable.Show();
        }
    }

    public void StartTour(int tourID, Guide guide)
    {
        guide.StartTour(tourID);
    }

    public void OptionsGuide(List<GuidedTour> tours, Guide guide)
    {
        string option = ViewVisitors.Show();
        int tourID;

        if (option.ToLower() == "v")
        {
            tourID = TourId.WhichTourId();
            foreach (var tour in tours)
            {
                if (tour.ID == tourID)
                {
                    ViewVisitorsTour(tourID, tour, guide);
                    return;
                }
            }
            Console.WriteLine("Tour not found!");
        }
        else if (option.ToLower() == "s")
        {
            tourID = TourId.WhichTourId();
            StartTour(tourID, guide);
        }
        else
        {
            WrongInput.Show();
        }
    }
}