public static class GuideController
{
    public static void ViewVisitorsTour()
    {
        int tourID = TourId.WhichTourId();

        Tour.OverviewVisitorsTour(tourID);

        string option = GuideOptions.Options();

        if (option.ToLower() == "a")
        {
            Guide.AddVisistorToTour(tourID);
        }
        else if (option.ToLower() == "r")
        {

        }
        else
        {
            WrongInput.Show();
        }
    }

    public static void StartTour()
    {
        int tourID = TourId.WhichTourId();

        Guide.StartTour(tourID);
    }

    public static void OptionsGuide()
    {
        string option = ViewVisitors.Show();

        if (option.ToLower() == "v")
        {
            ViewVisitorsTour();
        }
        else if (option.ToLower() == "s")
        {
            StartTour();
        }
        else
        {
            WrongInput.Show();
        }
    }
}
