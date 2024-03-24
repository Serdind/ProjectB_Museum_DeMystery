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
                    Date TEXT NOT NULL,
                    PRIMARY KEY (Id_Visitor, Id_Tour)
                );";

            string createVisitorTableCommand = @"
                CREATE TABLE IF NOT EXISTS Visitors (
                    Id INTEGER,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Password TEXT NOT NULL,
                    Phonenumber TEXT NOT NULL,
                    PRIMARY KEY (ID)
                );";

            using (var createTable = new SqliteCommand(createTourTableCommand, connection))
            {
                createTable.ExecuteNonQuery();
            }

            using (var createTable = new SqliteCommand(createVisitorInTourTableCommand, connection))
            {
                createTable.ExecuteNonQuery();
            }

            using (var createTable = new SqliteCommand(createVisitorTableCommand, connection))
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
            Visitor visitor = new Visitor(null,null,null,null);
            Console.WriteLine("Welcome to Het Depot!");
            Console.WriteLine("Create account(C)\nLogin(L)\nQuit(Q)");
            string choice = Console.ReadLine();

            if (choice.ToLower() == "c")
            {
                visitor.CreateAccount();
            }
            else if (choice.ToLower() == "l")
            {
                visitor.Login();
            }

            Console.WriteLine("Reservate(R)\nQuit(Q)");
            string option = Console.ReadLine();

            if (option.ToLower() == "r")
            {              
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

                        using (var connection = new SqliteConnection(connectionString))
                        {
                            connection.Open();

                            string insertVisitorDataCommand = @"
                            INSERT OR IGNORE INTO VisitorInTour (Id_Visitor, Id_Tour, Date) VALUES 
                                (@Id_Visitor, @Id_Tour, @Date);";

                            using (var insertData = new SqliteCommand(insertVisitorDataCommand, connection))
                            {
                                insertData.Parameters.AddWithValue("@Id_Visitor", visitor.Id);
                                insertData.Parameters.AddWithValue("@Id_Tour", tour.ID);
                                insertData.Parameters.AddWithValue("@Date", tour.Date);

                                insertData.ExecuteNonQuery();
                            }
                        }

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