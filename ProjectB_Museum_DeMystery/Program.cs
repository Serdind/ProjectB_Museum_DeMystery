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
                    Language TEXT NOT NULL,
                    Visitors INTEGER,
                    Guide TEXT NOT NULL
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
                    QR TEXT NOT NULL,
                    PRIMARY KEY (ID)
                );";
            
            string createDepartmentHeadTableCommand = @"
                CREATE TABLE IF NOT EXISTS DepartmentHead (
                    Id INTEGER,
                    Name TEXT NOT NULL,
                    QR TEXT NOT NULL,
                    PRIMARY KEY (ID)
                );";
            
            string createGuideTableCommand = @"
                CREATE TABLE IF NOT EXISTS Guide (
                    Id INTEGER,
                    Name TEXT NOT NULL,
                    QR TEXT NOT NULL,
                    PRIMARY KEY (ID)
                );";
            
            string createGuideInTourTableCommand = @"
                CREATE TABLE IF NOT EXISTS GuideInTour (
                    Id_Guide INTEGER,
                    Id_Tour INTEGER,
                    Date TEXT NOT NULL,
                    PRIMARY KEY (Id_Guide, Id_Tour)
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

            using (var createTable = new SqliteCommand(createDepartmentHeadTableCommand, connection))
            {
                createTable.ExecuteNonQuery();
            }

            using (var createTable = new SqliteCommand(createGuideTableCommand, connection))
            {
                createTable.ExecuteNonQuery();
            }

            using (var createTable = new SqliteCommand(createGuideInTourTableCommand, connection))
            {
                createTable.ExecuteNonQuery();
            }

            string insertDepartmentHeadDataCommand = @"
                    INSERT OR IGNORE INTO DepartmentHead (Id, Name, QR) VALUES 
                        (@Id, @Name, @Qr);";
            
            string insertGuideDataCommand = @"
                    INSERT OR IGNORE INTO Guide (Id, Name, QR) VALUES 
                        (@Id, @Name, @Qr);";

                using (var insertData = new SqliteCommand(insertGuideDataCommand, connection))
                {
                    insertData.Parameters.AddWithValue("@Id", 1);
                    insertData.Parameters.AddWithValue("@Name", "Casper");
                    insertData.Parameters.AddWithValue("@Qr", "4892579");

                    insertData.ExecuteNonQuery();
                }
            

                using (var insertData = new SqliteCommand(insertDepartmentHeadDataCommand, connection))
                {
                    insertData.Parameters.AddWithValue("@Id", 1);
                    insertData.Parameters.AddWithValue("@Name", "Frans");
                    insertData.Parameters.AddWithValue("@Qr", "6457823");

                    insertData.ExecuteNonQuery();
                }

            foreach (GuidedTour tour in Tours.guidedTour)
            {
                string insertTourDataCommand = @"
                    INSERT OR IGNORE INTO Tours (Id, Name, Date, StartingPoint, EndPoint, Language, Visitors, Guide) VALUES 
                        (@Id, @Name, @Date, @StartingPoint, @EndPoint, @Language, @Visitors, @Guide);";

                using (var insertData = new SqliteCommand(insertTourDataCommand, connection))
                {
                    insertData.Parameters.AddWithValue("@Id", tour.ID);
                    insertData.Parameters.AddWithValue("@Name", tour.Name);
                    insertData.Parameters.AddWithValue("@Date", tour.Date);
                    insertData.Parameters.AddWithValue("@StartingPoint", GuidedTour.StartingPoint);
                    insertData.Parameters.AddWithValue("@EndPoint", GuidedTour.EndPoint);
                    insertData.Parameters.AddWithValue("@Language", tour.Language);
                    insertData.Parameters.AddWithValue("@Visitors", tour.ReservedVisitors.Count());
                    insertData.Parameters.AddWithValue("@Guide", tour.NameGuide);

                    insertData.ExecuteNonQuery();
                }
            }

            Tours.AddGuide(1);
            Tours.AddGuide(2);
            Tours.AddGuide(3);
            Tours.AddGuide(4);
            Tours.AddGuide(5);
            Tours.AddGuide(6);
            Tours.AddGuide(7);
            Tours.AddGuide(8);
            Tours.AddGuide(9);
            
        }        
        bool running = true;

        while (running)
        {
            Visitor visitor = new Visitor(null);
            Console.WriteLine("Welcome to Het Depot!");
            Console.WriteLine("Login(L)\nQuit(Q)");
            string choice = Console.ReadLine();

            if (choice.ToLower() == "l")
            {
                Console.WriteLine("Scan your QR code:");
                string qr = Console.ReadLine();

                visitor.AccCreated(qr);

                string loginStatus = visitor.Login(qr);
                if (loginStatus == "Visitor")
                {
                    bool visitorRunning = true;
                    while (visitorRunning)
                    {
                        Console.WriteLine("Make reservation(E)\nMy reservations(M)\nCancel reservation(C)\nQuit(Q)");
                        string option = Console.ReadLine();

                        if (option.ToLower() == "e")
                        {
                            Tours.ReservateTour(visitor);
                        }
                        else if (option.ToLower() == "m")
                        {
                            visitor.ViewReservationsMade(visitor.Id);
                        }
                        else if (option.ToLower() == "c")
                        {
                            visitor.CancelReservation(visitor);
                        }
                        else if (option.ToLower() == "q")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong input. Try again.");
                        }
                    }
                }
                else if (loginStatus == "Admin")
                {
                    visitor.AdminMenu();
                }
                else if (loginStatus == "Guide")
                {
                    bool guideRunning = true;

                    while (guideRunning)
                    {
                        Console.WriteLine("My tours(M)\nQuit (Q)");
                        string option = Console.ReadLine();

                        if (option.ToLower() == "m")
                        {
                            Tours.guide.ViewTours(Tours.guide.Id);
                        }
                        else if (option.ToLower() == "q")
                        {
                            guideRunning = false;
                        }
                        else
                        {
                            Console.WriteLine("Wrong input. Try again.");
                        }
                    }
                }
            }
            else if (choice.ToLower() == "q")
            {
                running = false;
                continue;
            }
            else
            {
                Console.WriteLine("Wrong input. Try again.");
            }
        }
    }
}