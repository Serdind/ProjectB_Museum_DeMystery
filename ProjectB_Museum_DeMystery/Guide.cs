using Microsoft.Data.Sqlite;
using Spectre.Console;

class Guide : Person
{
    string connectionString = "Data Source=MyDatabase.db";
    public string Name;
    public Guide(string name, string qr) : base(qr)
    {
        Name = name;
    }

    public void ViewTours(int guideID)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectGuideInTourDataCommand = @"
                SELECT * FROM GuideInTour WHERE Id_Guide = @GuideID";

            using (var selectData = new SqliteCommand(selectGuideInTourDataCommand, connection))
            {
                selectData.Parameters.AddWithValue("@GuideID", guideID);

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

                            while (reader.Read())
                            {
                                string selectToursDataCommand = @"
                                    SELECT * FROM Tours WHERE Id = @TourID";

                                using (var selectData2 = new SqliteCommand(selectToursDataCommand, connection))
                                {
                                    selectData2.Parameters.AddWithValue("@TourID", reader["Id_Tour"].ToString());

                                    using (var reader2 = selectData2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            DateTime dateValue = Convert.ToDateTime(reader2["Date"]);
                                            string timeOnly = dateValue.ToString("HH:mm");
                                            string dateOnly = dateValue.ToShortDateString();

                                            table.AddRow(
                                                reader2["Id"].ToString(),
                                                reader2["Name"].ToString(),
                                                dateOnly,
                                                timeOnly,
                                                reader2["StartingPoint"].ToString(),
                                                reader2["EndPoint"].ToString(),
                                                reader2["Language"].ToString()
                                            );

                                            ctx.Refresh();
                                        }
                                    }
                                }
                            }
                        });
                }
            }
        }
    }
}