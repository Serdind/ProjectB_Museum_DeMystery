static class Tours
{
    public static readonly List<GuidedTour> guidedTour = new List<GuidedTour>();

    static Tours()
    {
        UpdateTours();
    }

    public static void UpdateTours()
    {
        DateTime currentDate = DateTime.Today;

        guidedTour.RemoveAll(tour => tour.Date.Date == currentDate.Date);

        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 16, 30, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 9, 30, 0), "Engels"));
    }
}
