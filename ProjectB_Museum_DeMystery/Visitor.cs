using Microsoft.Data.Sqlite;
using Spectre.Console;
using Newtonsoft.Json;

class Visitor : Person
{
    string connectionString = "Data Source=MyDatabase.db";
    
    public Visitor(string qr) : base(qr){}

    public bool Reservate(string tourID, Visitor visitor)
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

    public bool ViewReservationsMade(int visitorID)
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
                                return true;
                            }
                        }
                    }

                    if (!reservationsExist)
                    {
                        return false;
                    }
                }
            }
        }
        return false;
    }

    public void CancelReservation(Visitor visitor)
    {
        string connectionString = "Data Source=MyDatabase.db";

        if (visitor.ViewReservationsMade(visitor.Id))
        {
            Console.WriteLine("Which tour do you wanna cancel?");
            int tourid = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Are you sure you want to cancel your reservation? (Y/N)");
            string confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "y")
            {            
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string removeTourCommand = @"
                        DELETE FROM VisitorInTour
                        WHERE Id_Tour = @TourID;";

                    using (var deleteCommand = new SqliteCommand(removeTourCommand, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@TourID", tourid);
                        deleteCommand.ExecuteNonQuery();
                        Console.WriteLine("Reservation removed successfully");
                    }
                
                    string selectToursDataCommand = @"
                        SELECT * FROM Tours WHERE Id = @TourID";

                    using (var selectData = new SqliteCommand(selectToursDataCommand, connection))
                    {
                        selectData.Parameters.AddWithValue("@TourID", tourid);

                        using (var reader = selectData.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int currentVisitors = reader.GetInt32(reader.GetOrdinal("Visitors"));

                                string updateVisitorsCountCommand = @"
                                    UPDATE Tours
                                    SET Visitors = Visitors - 1
                                    WHERE Id = @TourID;";

                                using (var updateCommand = new SqliteCommand(updateVisitorsCountCommand, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@TourID", tourid);
                                    updateCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            else if (confirmation.ToLower() == "n")
            {
                Console.WriteLine("Reservation cancellation cancelled.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
            }
            Tours.OverviewTours(false);
            return;
        }
        else
        {
            Console.WriteLine("No reservations made.");
        }
    }
}