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

        for (int i = guidedTour.Count - 1; i >= 0; i--)
        {
            if (guidedTour[i].Date.Date == currentDate.Date)
            {
                guidedTour.RemoveAt(i);
            }
        }

        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 11, 30, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 00, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 30, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 14, 00, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 14, 30, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 15, 00, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 16, 00, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 16, 30, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 17, 00, 0), "Engels"));
    }
}