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

            if (option.ToLower() == "a")
            {
                string qr = QRVisitor.WhichVisitorQr();

                if (uniqueCodes.Contains(qr))
                {
                    guide.AddVisistorToTour(tourId, qr);
                }
                else
                {
                    CodeNotValid.Show();
                }
            }
            else if (option.ToLower() == "r")
            {
                string qr = QRVisitor.WhichVisitorQr();
                if (uniqueCodes.Contains(qr))
                {
                    guide.RemoveVisitorFromTour(tourId, qr);
                }
                else
                {
                    CodeNotValid.Show();
                }
            }
            else
            {
                WrongInput.Show();
            }
        }
        else
        {
            TourNotAvailable.Show();
        }
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
            TourNotFound.Show();
        }
        else if (option.ToLower() == "s")
        {
            tourID = TourId.WhichTourId();
            Tour.guide.StartTour(tourID);
        }
        else if (option.ToLower() == "b")
        {
            return;
        }
        else
        {
            WrongInput.Show();
        }
    }
}
