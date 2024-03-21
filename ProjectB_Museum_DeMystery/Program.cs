using System;
using Microsoft.Data.Sqlite;

class Program
{
    public static void Main()
    {
        string connectionString = "Data Source=MyDatabase.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string createTourTableCommand = @"
                CREATE TABLE IF NOT EXISTS Tours (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Date INTEGER,
                    StartingPoint TEXT NOT NULL,
                    EndPoint TEXT NOT NULL,
                    Language TEXT NOT NULL
                );";

            string createVisitorInTourTableCommand = @"
                CREATE TABLE IF NOT EXISTS VisitorInTour (
                    Id_Visitor INTEGER,
                    Id_Tour INTEGER,
                    PRIMARY KEY (Id_Visitor, Id_Tour)
                );";

            using (var createTable = new SqliteCommand(createTourTableCommand, connection))
            {
                createTable.ExecuteNonQuery();
            }

            using (var createTable = new SqliteCommand(createVisitorInTourTableCommand, connection))
            {
                createTable.ExecuteNonQuery();
            }

            foreach (GuidedTour tour in Tours.guidedTour)
            {
                string insertTourDataCommand = @"
                    INSERT OR IGNORE INTO Tours (Id, Name, Date, StartingPoint, EndPoint, Language) VALUES 
                        (@Id, @Name, @Date, @StartingPoint, @EndPoint, @Language);";

                using (var insertData = new SqliteCommand(insertTourDataCommand, connection))
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

            if (option.ToLower() == "r")
            {
                Console.WriteLine("Insert your full name:");
                string name = Console.ReadLine();
                Console.WriteLine("Insert your email:");
                string email = Console.ReadLine();
                Console.WriteLine("Insert your phonenumber:");
                string phonenumber = Console.ReadLine();


                Visitor visitor = new Visitor(name, email, phonenumber);
                
                Tours.OverviewTours();
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