using Spectre.Console;
using System;
using Microsoft.Data.Sqlite;

static class Tours
{
    public static readonly List<GuidedTour> guidedTour = new List<GuidedTour>();
    public static string connectionString = "Data Source=MyDatabase.db";
    public static Guide guide = new Guide("Casper");
    public static string maxParticipants = "13";

    static Tours()
    {
        UpdateTours(1);
    }

    public static void UpdateTours(int numberOfDays)
    {
        DateTime yesterday = DateTime.Today.AddDays(-1);
        RemoveToursFromDate(yesterday);
        RemoveVisitorInTourFromDate(yesterday);

        DateTime currentDate = DateTime.Today;

        for (int day = 0; day < numberOfDays; day++)
        {
            DateTime currentDay = currentDate.AddDays(day);

            ToursDay(currentDay);
        }
    }

    private static void ToursDay(DateTime date)
    {
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 11, 30, 0), "English", guide.Name));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 13, 00, 0), "English", guide.Name));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 13, 30, 0), "English", guide.Name));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 14, 00, 0), "English", guide.Name));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 14, 30, 0), "English", guide.Name));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 15, 00, 0), "English", guide.Name));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 16, 00, 0), "English", guide.Name));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 16, 30, 0), "English", guide.Name));
        guidedTour.Add(new GuidedTour("Museum tour", new DateTime(date.Year, date.Month, date.Day, 17, 00, 0), "English", guide.Name));
    }

    public static void OverviewTours()
    {
        DateTime currentDate = DateTime.Today;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectToursDataCommand = @"
                SELECT * FROM Tours WHERE Date(Date) = Date(@Date)";

            using (var selectData = new SqliteCommand(selectToursDataCommand, connection))
            {
                selectData.Parameters.AddWithValue("@Date", currentDate.Date);

                using (var reader = selectData.ExecuteReader())
                {
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
                            table.AddColumn("Guide");

                            while (reader.Read())
                            {
                                DateTime dateValue = Convert.ToDateTime(reader["Date"]);
                                string timeOnly = dateValue.ToString("HH:mm");
                                string dateOnly = dateValue.ToShortDateString();
                                string visitors = reader["Visitors"].ToString() == Tours.maxParticipants ? "Full" : reader["Visitors"].ToString();

                                table.AddRow(
                                    reader["Id"].ToString(),
                                    reader["Name"].ToString(),
                                    dateOnly,
                                    timeOnly,
                                    reader["StartingPoint"].ToString(),
                                    reader["EndPoint"].ToString(),
                                    reader["Language"].ToString(),
                                    visitors,
                                    reader["Guide"].ToString()
                                );

                                ctx.Refresh();
                            }
                        });
                }
            }
        }
    }

    private static void RemoveToursFromDate(DateTime date)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string deleteToursDataCommand = @"
                DELETE FROM Tours WHERE Date(Date) = Date(@Date)";

            using (var deleteData = new SqliteCommand(deleteToursDataCommand, connection))
            {
                deleteData.Parameters.AddWithValue("@Date", date.Date);
                deleteData.ExecuteNonQuery();
            }
        }
    }

    private static void RemoveVisitorInTourFromDate(DateTime date)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string deleteVisitorInTourDataCommand = @"
                DELETE FROM VisitorInTour WHERE Date(Date) = Date(@Date)";

            using (var deleteData = new SqliteCommand(deleteVisitorInTourDataCommand, connection))
            {
                deleteData.Parameters.AddWithValue("@Date", date.Date);
                deleteData.ExecuteNonQuery();
            }
        }
    }

    public static void ReservateTour(Visitor visitor)
    {
        OverviewTours();
        Console.WriteLine("Which tour? (ID)");
        int tourID = Convert.ToInt32(Console.ReadLine());

        visitor.Reservate(tourID, visitor);
    }
}