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
public class GuideOptions : View
{
    public static string Options()
    {
        Console.WriteLine("Add visitor(A)\nRemove visitor(R)");
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
        Console.WriteLine("My tours(M)");
        return ReadLineString();
    }
}
