using System;
using Microsoft.Data.Sqlite;
using Spectre.Console;

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
                    Visitors INTEGER
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
            
            string createDepartmentHeadTableCommand = @"
                CREATE TABLE IF NOT EXISTS DepartmentHead (
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

            using (var createTable = new SqliteCommand(createDepartmentHeadTableCommand, connection))
            {
                createTable.ExecuteNonQuery();
            }

            string insertDepartmentHeadDataCommand = @"
                    INSERT OR IGNORE INTO DepartmentHead (Id, Name, Email, Password, Phonenumber) VALUES 
                        (@Id, @Name, @Email, @Password, @Phonenumber);";

                using (var insertData = new SqliteCommand(insertDepartmentHeadDataCommand, connection))
                {
                    insertData.Parameters.AddWithValue("@Id", 1);
                    insertData.Parameters.AddWithValue("@Name", "John");
                    insertData.Parameters.AddWithValue("@Email", "John@hetdepot.com");
                    insertData.Parameters.AddWithValue("@Password", "Password123");
                    insertData.Parameters.AddWithValue("@Phonenumber", 01111132);

                    insertData.ExecuteNonQuery();
                }

            foreach (GuidedTour tour in Tours.guidedTour)
            {
                string insertTourDataCommand = @"
                    INSERT OR IGNORE INTO Tours (Id, Name, Date, StartingPoint, EndPoint, Language, Visitors) VALUES 
                        (@Id, @Name, @Date, @StartingPoint, @EndPoint, @Language, @Visitors);";

                using (var insertData = new SqliteCommand(insertTourDataCommand, connection))
                {
                    insertData.Parameters.AddWithValue("@Id", tour.ID);
                    insertData.Parameters.AddWithValue("@Name", tour.Name);
                    insertData.Parameters.AddWithValue("@Date", tour.Date);
                    insertData.Parameters.AddWithValue("@StartingPoint", GuidedTour.StartingPoint);
                    insertData.Parameters.AddWithValue("@EndPoint", GuidedTour.EndPoint);
                    insertData.Parameters.AddWithValue("@Language", tour.Language);
                    insertData.Parameters.AddWithValue("@Visitors", tour.ReservedVisitors.Count());

                    insertData.ExecuteNonQuery();
                }
            }
        }
        
        bool running = true;

        while (running)
        {
            Console.WriteLine("Welcome to Het Depot!");
            Console.WriteLine("Create account(C)\nLogin(L)\nQuit(Q)");
            string choice = Console.ReadLine();

            if (choice.ToLower() == "c")
            {
                Visitor visitor = new Visitor(null,null,null,null);
                visitor.CreateAccount();
            }
            else if (choice.ToLower() == "l")
            {
                Visitor visitor = new Visitor(null,null,null,null);
                string loginStatus = visitor.Login();
                if (loginStatus == "Visitor")
                {
                    Console.WriteLine("Make reservation (R)\nQuit (Q)");
                    string option = Console.ReadLine();

                    if (option.ToLower() == "r")
                    {
                        Tours.ReservateTour(visitor);
                        running = false;
                    }
                    else if (option.ToLower() == "q")
                    {
                        running = false;
                    }
                }
                else if (loginStatus == "Admin")
                {
                    bool adminRunning = true;

                    while (adminRunning)
                    {
                        Console.WriteLine("Add tour (A)\nEdit tour (E)\nRemove tour (R)\nQuit (Q)");
                        string option = Console.ReadLine();

                        if (option.ToLower() == "a")
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

                            GuidedTour guidedTour = new GuidedTour(name, date, language);

                            using (var connection = new SqliteConnection(connectionString))
                            {
                                connection.Open();

                                string insertTourDataCommand = @"
                                    INSERT OR IGNORE INTO Tours (Id, Name, Date, StartingPoint, EndPoint, Language, Visitors) VALUES 
                                        (@Id, @Name, @Date, @StartingPoint, @EndPoint, @Language, @Visitors);";

                                using (var insertData = new SqliteCommand(insertTourDataCommand, connection))
                                {
                                    insertData.Parameters.AddWithValue("@Id", guidedTour.ID);
                                    insertData.Parameters.AddWithValue("@Name", guidedTour.Name);
                                    insertData.Parameters.AddWithValue("@Date", guidedTour.Date);
                                    insertData.Parameters.AddWithValue("@StartingPoint", GuidedTour.StartingPoint);
                                    insertData.Parameters.AddWithValue("@EndPoint", GuidedTour.EndPoint);
                                    insertData.Parameters.AddWithValue("@Language", guidedTour.Language);
                                    insertData.Parameters.AddWithValue("@Visitors", guidedTour.ReservedVisitors.Count());

                                    insertData.ExecuteNonQuery();
                                }
                            }
                        }
                        else if (option.ToLower() == "e")
                        {
                            Tours.OverviewTours();
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

                                                while (reader.Read())
                                                {
                                                    DateTime dateValue = Convert.ToDateTime(reader["Date"]);
                                                    string timeOnly = dateValue.ToString("HH:mm");
                                                    string dateOnly = dateValue.ToShortDateString();
                                                    string visitors = reader["Visitors"].ToString() == "13" ? "Full" : reader["Visitors"].ToString();

                                                    table.AddRow(
                                                        reader["Id"].ToString(),
                                                        reader["Name"].ToString(),
                                                        dateOnly,
                                                        timeOnly,
                                                        reader["StartingPoint"].ToString(),
                                                        reader["EndPoint"].ToString(),
                                                        reader["Language"].ToString(),
                                                        visitors
                                                    );

                                                    ctx.Refresh();
                                                }
                                            });
                                        
                                        Console.WriteLine("What do you want to change?\nName(N)\nDate(D)\nLanguage(L)\nVisitors(V)");
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
                                            Console.WriteLine("Date (Y-M-D H:M:S): ");
                                            string dateString = Console.ReadLine();

                                            DateTime date;
                                            if (!DateTime.TryParse(dateString, out date))
                                            {
                                                Console.WriteLine("Invalid date format. Please enter a valid date.");
                                                return;
                                            }

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
                                    }
                                }
                            }
                        }
                        else if (option.ToLower() == "r")
                        {
                            Tours.OverviewTours();
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
                    }
                }
            }
            else if (choice.ToLower() == "q")
            {
                running = false;
                continue;
            }
        }
    }
}