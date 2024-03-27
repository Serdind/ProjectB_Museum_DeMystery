using Microsoft.Data.Sqlite;
using Spectre.Console;

class Guide : Person
{
    string connectionString = "Data Source=MyDatabase.db";
    public Guide(string name, string email, string password, string phonenumber) : base(name, email, password, phonenumber){}

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
                    bool reservationsExist = false;

                    while (reader.Read())
                    {
                        string selectToursDataCommand = @"
                            SELECT * FROM Tours WHERE Id = @TourID";

                        using (var selectData2 = new SqliteCommand(selectToursDataCommand, connection))
                        {
                            selectData2.Parameters.AddWithValue("@TourID", reader["Id_Tour"].ToString());

                            using (var reader2 = selectData2.ExecuteReader())
                            {
                                var table = new Table().LeftAligned();

                                table.AddColumn("ID");
                                table.AddColumn("Name");
                                table.AddColumn("Date");
                                table.AddColumn("Time");
                                table.AddColumn("StartingPoint");
                                table.AddColumn("EndPoint");
                                table.AddColumn("Language");

                                while (reader2.Read())
                                {
                                    reservationsExist = true;
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
                                }
                                AnsiConsole.Render(table);
                            }
                        }
                    }

                    if (!reservationsExist)
                    {
                        Console.WriteLine("No reservations made.");
                    }
                }
            }
        }
    }
}