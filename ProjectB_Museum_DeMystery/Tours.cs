static class Tours
{
    public static readonly List<GuidedTour> guidedTour = new List<GuidedTour>();
    public static GuidedTour tour;

    static Tours()
    {
        UpdateTours(1);
    }

    public static void UpdateTours(int numberOfDays)
    {
        DateTime currentDate = DateTime.Today;

        for (int day = 0; day < numberOfDays; day++)
        {
            DateTime currentDay = currentDate.AddDays(day);

            ToursDay(currentDay);
        }
    }

    private static void ToursDay(DateTime date)
    {
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 11, 30, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 13, 00, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 13, 30, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 14, 00, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 14, 30, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 15, 00, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 16, 00, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 16, 30, 0), "Engels"));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 17, 00, 0), "Engels"));
    }

    public static void OverviewTours()
    {
        foreach (GuidedTour tour in guidedTour)
        {
            string timeOnly = tour.Date.ToString("HH:mm");
            string dateOnly = tour.Date.ToShortDateString();

            Console.WriteLine($"ID: {tour.ID}");
            Console.WriteLine($"Name: {tour.Name}");
            Console.WriteLine($"Language: {tour.Language}");
            Console.WriteLine($"Date: {dateOnly}");
            Console.WriteLine($"Time: {timeOnly}");
            if (tour.ReservedVisitors.Count() == 13)
            {
                Console.WriteLine($"Visitors: {tour.ReservedVisitors.Count()} (Full)\n");
            }
            else
            {
                Console.WriteLine($"Visitors: {tour.ReservedVisitors.Count()}\n");
            }
        }
    }
}