using Microsoft.Data.Sqlite;
using Spectre.Console;

class Visitor : Person
{
    string connectionString = "Data Source=MyDatabase.db";
    
    public Visitor(string name) : base(name){}

    public bool Reservate(int tourID, Visitor visitor)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectToursDataCommand = @"
                SELECT * FROM Tours WHERE Id = @TourID";

            using (var selectData = new SqliteCommand(selectToursDataCommand, connection))
            {
                selectData.Parameters.AddWithValue("@TourID", tourID);

                using (var reader = selectData.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int currentVisitors = reader.GetInt32(reader.GetOrdinal("Visitors"));

                        if (currentVisitors != 13)
                        {
                            string updateVisitorsCountCommand = @"
                                UPDATE Tours
                                SET Visitors = Visitors + 1
                                WHERE Id = @TourID;";

                            using (var updateCommand = new SqliteCommand(updateVisitorsCountCommand, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@TourID", tourID);
                                updateCommand.ExecuteNonQuery();
                            }

                            string insertVisitorDataCommand = @"
                                INSERT OR IGNORE INTO VisitorInTour (Id_Visitor, Id_Tour, Date) VALUES 
                                    (@Id_Visitor, @Id_Tour, @Date);";

                            using (var insertData = new SqliteCommand(insertVisitorDataCommand, connection))
                            {
                                insertData.Parameters.AddWithValue("@Id_Visitor", visitor.Id);
                                insertData.Parameters.AddWithValue("@Id_Tour", reader["Id"].ToString());
                                insertData.Parameters.AddWithValue("@Date", reader["Date"].ToString());

                                insertData.ExecuteNonQuery();
                                return true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Tour is full");
                            Tours.ReservateTour(visitor);
                        }
                    }
                }
            }
        }
        Console.WriteLine("Tour not found. Try again!");
        Tours.ReservateTour(visitor);
        return false;
    }

    public void ViewReservationsMade(int visitorID)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectVisitorInTourDataCommand = @"
                SELECT * FROM VisitorInTour WHERE Id_Visitor = @VisitorID";

            using (var selectData = new SqliteCommand(selectVisitorInTourDataCommand, connection))
            {
                selectData.Parameters.AddWithValue("@VisitorID", visitorID);

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