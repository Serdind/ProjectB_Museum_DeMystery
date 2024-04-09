using System;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;


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
                    insertData.Parameters.AddWithValue("@Id", Tours.guide.Id);
                    insertData.Parameters.AddWithValue("@Name", Tours.guide.Name);
                    insertData.Parameters.AddWithValue("@Qr", Tours.guide.QR);

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

            AddGuide(1);
            AddGuide(2);
            AddGuide(3);
            AddGuide(4);
            AddGuide(5);
            AddGuide(6);
            AddGuide(7);
            AddGuide(8);
            AddGuide(9);
            
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
                            running = false;
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
                    bool adminRunning = true;

                    while (adminRunning)
                    {
                        Console.WriteLine("\nOverview tours(T)\nAdd tour (A)\nEdit tour (E)\nRemove tour (R)\nQuit (Q)");
                        string option = Console.ReadLine();
                        
                        if (option.ToLower() == "t")
                        {
                            Tours.OverviewTours(true);
                        }
                        else if (option.ToLower() == "a")
                        {
                            Console.WriteLine("Name: ");
                            string name = Console.ReadLine();
                            Console.WriteLine("Date (Y-M-D H:M:S): ");
                            string dateString = Console.ReadLine();

                            DateTime date;
                            if (!DateTime.TryParse(dateString, out date))
                            {
                                Console.WriteLine("Invalid date format. Please enter a valid date.");
                                return;
                            }

                            Console.WriteLine("Language: ");
                            string language = Console.ReadLine();

                            using (var connection = new SqliteConnection(connectionString))
                            {
                                connection.Open();

                                string insertTourDataCommand = @"
                                    INSERT OR IGNORE INTO Tours (Id, Name, Date, StartingPoint, EndPoint, Language, Visitors, Guide) VALUES 
                                        (@Id, @Name, @Date, @StartingPoint, @EndPoint, @Language, @Visitors, @Guide);";

                                using (var insertData = new SqliteCommand(insertTourDataCommand, connection))
                                {
                                    insertData.Parameters.AddWithValue("@Name", name);
                                    insertData.Parameters.AddWithValue("@Date", date);
                                    insertData.Parameters.AddWithValue("@StartingPoint", GuidedTour.StartingPoint);
                                    insertData.Parameters.AddWithValue("@EndPoint", GuidedTour.EndPoint);
                                    insertData.Parameters.AddWithValue("@Language", language);
                                    insertData.Parameters.AddWithValue("@Visitors", 0);
                                    insertData.Parameters.AddWithValue("@Guide", Tours.guide.Name);

                                    insertData.ExecuteNonQuery();
                                }
                            }
                        }
                        else if (option.ToLower() == "e")
                        {
                            Tours.OverviewTours(true);
                            Console.WriteLine("Tour (Id): ");
                            string id = Console.ReadLine();

                            using (var connection = new SqliteConnection(connectionString))
                            {
                                connection.Open();

                                string selectTourDataCommand = @"
                                    SELECT * FROM Tours WHERE Id = @TourID";
                                
                                using (var selectData = new SqliteCommand(selectTourDataCommand, connection))
                                {
                                    selectData.Parameters.AddWithValue("@TourID", id);

                                    using (var reader = selectData.ExecuteReader())
                                    {
                                        var table = new Table().LeftAligned();

                                        AnsiConsole.Live(table)
                                            .AutoClear(false)
                                            .Overflow(VerticalOverflow.Ellipsis)
                                            .Cropping(VerticalOverflowCropping.Top)
                                            .Start(ctx =>
                                            {
                                                table.AddColumn("ID");
                                                table.AddColumn("Name");
                                                table.AddColumn("Date");
                                                table.AddColumn("Time");
                                                table.AddColumn("StartingPoint");
                                                table.AddColumn("EndPoint");
                                                table.AddColumn("Language");
                                                table.AddColumn("Visitors");
                                                table.AddColumn("Guide");

                                                while (reader.Read())
                                                {
                                                    DateTime dateValue = Convert.ToDateTime(reader["Date"]);
                                                    string timeOnly = dateValue.ToString("HH:mm");
                                                    string dateOnly = dateValue.ToShortDateString();
                                                    string visitors = reader["Visitors"].ToString() == Tours.maxParticipants ? "Full" : reader["Visitors"].ToString();

                                                    table.AddRow(
                                                        reader["Id"].ToString(),
                                                        reader["Name"].ToString(),
                                                        dateOnly,
                                                        timeOnly,
                                                        reader["StartingPoint"].ToString(),
                                                        reader["EndPoint"].ToString(),
                                                        reader["Language"].ToString(),
                                                        visitors,
                                                        reader["Guide"].ToString()
                                                    );

                                                    ctx.Refresh();
                                                }
                                            });
                                        
                                        Console.WriteLine("What do you want to change?\nName(N)\nDate(D)\nTime(T)\nLanguage(L)\nVisitors(V)");
                                        string change = Console.ReadLine();

                                        if (change.ToLower() == "n")
                                        {
                                            Console.WriteLine("Name:");
                                            string name = Console.ReadLine();

                                            string updateNameCommand = @"
                                            UPDATE Tours
                                            SET Name = @Name
                                            WHERE Id = @TourID;";

                                            using (var updateCommand = new SqliteCommand(updateNameCommand, connection))
                                            {
                                                updateCommand.Parameters.AddWithValue("@Name", name);
                                                updateCommand.Parameters.AddWithValue("@TourID", id);
                                                updateCommand.ExecuteNonQuery();
                                                Console.WriteLine($"Name set to {name}");
                                            }
                                        }
                                        else if (change.ToLower() == "d")
                                        {
                                            Console.WriteLine("Date (Y-M-D): ");
                                            string dateString = Console.ReadLine();

                                            DateTime date;
                                            if (!DateTime.TryParse(dateString, out date))
                                            {
                                                Console.WriteLine("Invalid date format. Please enter a valid date.");
                                                return;
                                            }

                                            date = date.Date;

                                            string updateDateCommand = @"
                                            UPDATE Tours
                                            SET Date = @Date
                                            WHERE Id = @TourID;";

                                            using (var updateCommand = new SqliteCommand(updateDateCommand, connection))
                                            {
                                                updateCommand.Parameters.AddWithValue("@Date", date);
                                                updateCommand.Parameters.AddWithValue("@TourID", id);
                                                updateCommand.ExecuteNonQuery();
                                                Console.WriteLine($"Date set to {date}");
                                            }
                                        }
                                        else if (change.ToLower() == "t")
                                        {
                                            Console.WriteLine("Time (H:M:S): ");
                                            string timeString = Console.ReadLine();

                                            DateTime time;
                                            if (!DateTime.TryParseExact(timeString, "H:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                                            {
                                                Console.WriteLine("Invalid time format. Please enter a valid time (H:M:S).");
                                                return;
                                            }

                                            DateTime currentDate = DateTime.Now.Date;

                                            DateTime updatedDateTime = currentDate.Add(time.TimeOfDay);

                                            string updateDateTimeCommand = @"
                                            UPDATE Tours
                                            SET Date = @Date
                                            WHERE Id = @TourID;";

                                            using (var updateCommand = new SqliteCommand(updateDateTimeCommand, connection))
                                            {
                                                updateCommand.Parameters.AddWithValue("@Date", updatedDateTime);
                                                updateCommand.Parameters.AddWithValue("@TourID", id);
                                                updateCommand.ExecuteNonQuery();
                                                Console.WriteLine($"Time set to {updatedDateTime.TimeOfDay}");
                                            }
                                        }
                                        else if (change.ToLower() == "l")
                                        {
                                            Console.WriteLine("Language: ");
                                            string language = Console.ReadLine();

                                            string updateLanguageCommand = @"
                                            UPDATE Tours
                                            SET Language = @Language
                                            WHERE Id = @TourID;";

                                            using (var updateCommand = new SqliteCommand(updateLanguageCommand, connection))
                                            {
                                                updateCommand.Parameters.AddWithValue("@Language", language);
                                                updateCommand.Parameters.AddWithValue("@TourID", id);
                                                updateCommand.ExecuteNonQuery();
                                                Console.WriteLine($"Language set to {language}");
                                            }
                                        }
                                        else if (change.ToLower() == "v")
                                        {
                                            Console.WriteLine("Visitors: ");
                                            int visitors = Convert.ToInt32(Console.ReadLine());

                                            string updateVisitorsCommand = @"
                                            UPDATE Tours
                                            SET Visitors = @Visitors
                                            WHERE Id = @TourID;";

                                            using (var updateCommand = new SqliteCommand(updateVisitorsCommand, connection))
                                            {
                                                updateCommand.Parameters.AddWithValue("@Visitors", visitors);
                                                updateCommand.Parameters.AddWithValue("@TourID", id);
                                                updateCommand.ExecuteNonQuery();
                                                Console.WriteLine($"Visitors set to {visitors}");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Wrong input. Try again.");
                                        }
                                    }
                                }
                            }
                        }
                        else if (option.ToLower() == "r")
                        {
                            Tours.OverviewTours(true);
                            Console.WriteLine("Tour (Id): ");
                            string id = Console.ReadLine();

                            using (var connection = new SqliteConnection(connectionString))
                            {
                                connection.Open();

                                string removeTourCommand = @"
                                    DELETE FROM Tours
                                    WHERE Id = @TourID;";

                                using (var deleteCommand = new SqliteCommand(removeTourCommand, connection))
                                {
                                    deleteCommand.Parameters.AddWithValue("@TourID", id);
                                    deleteCommand.ExecuteNonQuery();
                                    Console.WriteLine("Tour removed successfully");
                                }
                            }
                        }
                        else if (option.ToLower() == "q")
                        {
                            adminRunning = false;
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Wrong input. Try again.");
                        }
                    }
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

    public static void AddGuide(long tourID)
    {
        string connectionString = "Data Source=MyDatabase.db";

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
                        int guideId = Tours.guide.Id;
                        string guideName = Tours.guide.Name;

                        string insertGuideInTourDataCommand = @"
                                INSERT OR IGNORE INTO GuideInTour (Id_Guide, Id_Tour, Date) VALUES 
                                    (@Id_Guide, @Id_Tour, @Date);";

                        using (var insertData = new SqliteCommand(insertGuideInTourDataCommand, connection))
                        {
                            insertData.Parameters.AddWithValue("@Id_Guide", guideId);
                            insertData.Parameters.AddWithValue("@Id_Tour", tourID);
                            insertData.Parameters.AddWithValue("@Date", reader["Date"]);

                            insertData.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}