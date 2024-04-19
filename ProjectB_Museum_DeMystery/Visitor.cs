using Microsoft.Data.Sqlite;
using Spectre.Console;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Visitor : Person
{
    string connectionString = "Data Source=MyDatabase.db";
    
    private static int lastId = 1;
    public int Id;
    public int TourId;

    public Visitor(int tourId, string qr) : base(qr)
    {
        Id = lastId++;
        TourId = tourId;
    }

    public bool Reservate(int tourID, Visitor visitor)
    {
        Tours.visitors.Clear();

        if (ViewReservationsMade(visitor.Id))
        {
            Console.WriteLine("You already reserved a tour for today.\n");
            return false;
        }

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName1 = "visitors.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null)
            {
                Tours.AddVisitorToJSON(tourID, visitor.QR);

                string message = $"Reservation successful. You have reserved the following tour:\n" +
                                 $"Tour: {tour.Name}\n" +
                                 $"Date: {tour.Date.ToShortDateString()}\n" +
                                 $"Time: {tour.Date.ToString("HH:mm")}\n" +
                                 $"Language: {tour.Language}\n";
                Console.WriteLine(message);
                return true;
            }
            else
            {
                Console.WriteLine("Tour is not available.");
            }
        }

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
            Console.WriteLine("Which tour do you wanna cancel?(ID)");
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
            return;
        }
        else
        {
            Console.WriteLine("No reservations made.");
        }
    }
}