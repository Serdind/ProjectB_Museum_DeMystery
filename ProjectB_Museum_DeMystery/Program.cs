using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

class Program
{
    public static void Main()
    {
        Tours.UpdateTours();

        Tours.OverviewTours(false);

        Tours.AddAdminToJSON();

        Tours.AddGuideToJSON();
        

        string connectionString = "Data Source=MyDatabase.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

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
            
        }        
        bool running = true;

        while (running)
        {
            Visitor visitor = new Visitor(0,null);
            Console.WriteLine("Welcome to Het Depot!");
            Console.WriteLine("select language  /   selecteer taal");
            Console.WriteLine("English(E)   /   Nederlands(N)");
            string language = Console.ReadLine();
            if (language.ToLower() == "e")
            {
                Console.WriteLine("Login(L)\nQuit(Q)");
                string choice = Console.ReadLine();

                if (choice.ToLower() == "l")
                {
                    Console.WriteLine("Scan your QR code:");
                    string qr = Console.ReadLine();

                    visitor.QR = qr;

                    visitor.AccCreated(qr);

                    
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
            else if (language.ToLower() == "n")
            {
                Console.WriteLine("Login(L)\nAfsluiten(Q)");
                string choice = Console.ReadLine();

                if (choice.ToLower() == "l")
                {
                    Console.WriteLine("Scan uw QR code:");
                    string qr = Console.ReadLine();

                    visitor.AccCreated(qr);

                    string loginStatus = visitor.Login(qr);
                    if (loginStatus == "Visitor")
                    {
                        bool visitorRunning = true;
                        while (visitorRunning)
                        {
                            Console.WriteLine("Reservering maken(E)\nMijn reserveringen(M)\nreservering annuleren(C)\nVerlaten(Q)");
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
                                Console.WriteLine("ingevoerd antwoord onjuist, probeer opnieuw");
                            }
                        }
                    }
                    else if (loginStatus == "Admin")
                    {
                        visitor.AdminMenu(2);
                    }
                    else if (loginStatus == "Guide")
                    {
                        bool guideRunning = true;

                        while (guideRunning)
                        {
                            Console.WriteLine("Mijn rondleidingen(M)\nVerlaten (Q)");
                            string option = Console.ReadLine();

                            if (option.ToLower() == "m")
                            {
                                Tours.guide.ViewTours("Casper");
                            }
                            else if (option.ToLower() == "q")
                            {
                                guideRunning = false;
                            }
                            else
                            {
                                Console.WriteLine("ingevoerd antwoord onjuist, probeer opnieuw");
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
                    Console.WriteLine("ingevoerd antwoord onjuist, probeer opnieuw");
                }
            }
            else
            {
                Console.WriteLine("wrong input");
            }
        }
    }
}