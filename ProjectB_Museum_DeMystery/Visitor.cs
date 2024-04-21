using Microsoft.Data.Sqlite;
using Spectre.Console;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Visitor : Person
{
    
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
        if (ReservationMade(visitor.QR))
        {
            Console.WriteLine("You already made a reservation for today.");
            return false;
        }

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var tour = tours.FirstOrDefault(t => t.ID == tourID);

            if (tour != null)
            {
                Tours.AddVisitorToJSON(tourID, visitor.QR);

                tour.ReservedVisitors.Add(visitor);
                visitor.TourId = tour.ID;

                string updatedJson = JsonConvert.SerializeObject(tours, Formatting.Indented);

                File.WriteAllText(filePath, updatedJson);

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

    public bool ViewReservationsMade(string qr)
    {
        List<Visitor> visitors = Tours.LoadVisitorsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        if (visitor != null)
        {
            List<GuidedTour> tours = Tours.LoadToursFromFile();

            GuidedTour tour = tours.FirstOrDefault(t => t.ID == visitor.TourId);

            if (tour != null)
            {
                string message = $"Tour: {tour.Name}\n" +
                                $"Date: {tour.Date.ToShortDateString()}\n" +
                                $"Time: {tour.Date.ToString("HH:mm")}\n" +
                                $"Language: {tour.Language}\n";

                Console.WriteLine(message);
                return true;
            }
        }
        return false;
    }

    public bool ReservationMade(string qr)
    {
        List<Visitor> visitors = Tours.LoadVisitorsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        return visitor != null;
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