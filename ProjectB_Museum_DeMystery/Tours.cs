using Spectre.Console;
using System;

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
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 11, 30, 0), "English"));
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

            var table = new Table().LeftAligned();

            AnsiConsole.Live(table)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Ellipsis)
                .Cropping(VerticalOverflowCropping.Top)
                .Start(ctx =>
                {
                    table.AddColumn("ID");
                    table.AddColumn("Name");
                    table.AddColumn("Date");
                    table.AddColumn("Time");
                    table.AddColumn("StartingPoint");
                    table.AddColumn("EndPoint");
                    table.AddColumn("Language");
                    table.AddColumn("Visitors");
                    ctx.Refresh();

                    table.AddRow(
                        tour.ID.ToString(),
                        tour.Name,
                        dateOnly,
                        timeOnly,
                        GuidedTour.StartingPoint,
                        GuidedTour.EndPoint,
                        tour.Language,
                        tour.ReservedVisitors.Count() < tour.MaxParticipants ? tour.ReservedVisitors.Count().ToString() : "Full");
                        
                    ctx.Refresh();                    
                });
        }
    }
}