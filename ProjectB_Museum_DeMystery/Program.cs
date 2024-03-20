using System;
using Microsoft.Data.Sqlite;

class Program
{
    public static void Main()
    {
        string connectionString = "Data Source=MyDatabase.db";

        // Create a connection
        using (var connection = new SqliteConnection(connectionString))
        {
            // Open the connection
            connection.Open();

            string createTableCommand = @"
                CREATE TABLE IF NOT EXISTS Tours (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Date INTEGER,
                    StartingPoint TEXT NOT NULL,
                    EndPoint TEXT NOT NULL,
                    Language TEXT NOT NULL
                );";
            using (var createTable = new SqliteCommand(createTableCommand, connection))
            {
                createTable.ExecuteNonQuery();
            }

            foreach (GuidedTour tour in Tours.guidedTour)
            {
                string insertDataCommand = @"
                    INSERT INTO Tours (Id, Name, Date, StartingPoint, EndPoint, Language) VALUES 
                        (@Id, @Name, @Date, @StartingPoint, @EndPoint, @Language);";

                using (var insertData = new SqliteCommand(insertDataCommand, connection))
                {
                    insertData.Parameters.AddWithValue("@Id", tour.ID);
                    insertData.Parameters.AddWithValue("@Name", tour.Name);
                    insertData.Parameters.AddWithValue("@Date", tour.Date);
                    insertData.Parameters.AddWithValue("@StartingPoint", GuidedTour.StartingPoint);
                    insertData.Parameters.AddWithValue("@EndPoint", GuidedTour.EndPoint);
                    insertData.Parameters.AddWithValue("@Language", tour.Language);

                    insertData.ExecuteNonQuery();
                }
            }
        }
        
        bool running = true;

        while (running)
        {
            Console.WriteLine("Reservate(R)\nQuit(Q)");
            string option = Console.ReadLine();

            Console.WriteLine("Insert your full name:");
            string name = Console.ReadLine();
            Console.WriteLine("Insert your email:");
            string email = Console.ReadLine();
            Console.WriteLine("Insert your phonenumber:");
            string phonenumber = Console.ReadLine();


            Visitor visitor = new Visitor(name, email, phonenumber);
            
            Tours.OverviewTours();

            if (option.ToLower() == "r")
            {
                Console.WriteLine("Which tour? (ID)");
                int tourID = Convert.ToInt32(Console.ReadLine());
                bool tourFound = false;

                foreach (GuidedTour tour in Tours.guidedTour)
                {
                    if (tourID == tour.ID)
                    {
                        tour.PlaceReservation(tourID, visitor);
                        tourFound = true;
                        break;
                    }
                }

                if (!tourFound)
                {
                    Console.WriteLine("Tour not found.\n");
                }
            }
            else if (option.ToLower() == "q")
            {
                running = false;
                continue;
            }
        }
    }
}