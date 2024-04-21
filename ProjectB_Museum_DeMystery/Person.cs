using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

class Person
{

    public string QR;
    public int Id;
    string connectionString = "Data Source=MyDatabase.db";

    public Person(string qr)
    {
        QR = qr;
    }

    public string Login(string qr)
    {
        List<Visitor> visitors = Tours.LoadVisitorsFromFile();
        List<Guide> guides = Tours.LoadGuidesFromFile();
        List<DepartmentHead> admins = Tours.LoadAdminsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        if (visitor != null)
        {
            Console.WriteLine($"Logged in as: {visitor.QR}");
            return "Visitor";
        }

        Guide guide = guides.FirstOrDefault(v => v.QR == qr);

        if (guide != null)
        {
            Console.WriteLine($"Logged in as: {guide.Name}");
            return "Guide";
        }

        DepartmentHead admin = admins.FirstOrDefault(v => v.QR == qr);

        if (admin != null)
        {
            Console.WriteLine($"Logged in as: {admin.Name}");
            return "Admin";
        }
        return "None";
    }

    public bool AccCreated(string qr)
    {
        List<Guide> guides = Tours.LoadGuidesFromFile();
        List<DepartmentHead> admins = Tours.LoadAdminsFromFile();

        bool isGuide = guides.Any(g => g.QR == qr);
        bool isAdmin = admins.Any(a => a.QR == qr);

        if (isGuide)
        {
            return false;
        }
        else if (isAdmin)
        {
            return false;
        }
        else
        {
            Visitor visitor = new Visitor(0, qr);
            return true;
        }
    }

    public void AdminMenu(int languageSelection)
    {
        if (languageSelection == 1)
        {
            bool adminRunning = true;

            while (adminRunning)
            {
                Console.WriteLine("Overview tours(T)\nAdd tour (A)\nEdit tour (E)\nRemove tour (R)\nQuit (Q)");
                string option = Console.ReadLine();
                
                if (option.ToLower() == "t")
                {
                    Tours.OverviewTours(true);
                }
                else if (option.ToLower() == "a")
                {
                    Console.WriteLine("Name:");
                    string name = Console.ReadLine();
                    
                    DateTime date = DateTime.MinValue;
                    bool dateFormat = true;
                    
                    while (dateFormat)
                    {
                        Console.WriteLine("\nDate (Y-M-D H:M:S): ");
                        string dateString = Console.ReadLine();
                        
                        if (!DateTime.TryParse(dateString, out date))
                        {
                            Console.WriteLine("Invalid date format. Please enter a valid date.");
                        }
                        else
                        {
                            dateFormat = false;
                        }
                    }

                    Console.WriteLine("\nLanguage: ");
                    string language = Console.ReadLine();

                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();

                        string insertTourDataCommand = @"
                            INSERT OR IGNORE INTO Tours (Name, Date, StartingPoint, EndPoint, Language, Visitors, Guide) VALUES 
                                (@Name, @Date, @StartingPoint, @EndPoint, @Language, @Visitors, @Guide);";

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

                                            table.AddRow(
                                                reader["Id"].ToString(),
                                                reader["Name"].ToString(),
                                                dateOnly,
                                                timeOnly,
                                                reader["StartingPoint"].ToString(),
                                                reader["EndPoint"].ToString(),
                                                reader["Language"].ToString(),
                                                reader["Guide"].ToString()
                                            );

                                            ctx.Refresh();
                                        }
                                    });
                                
                                Console.WriteLine("What do you want to change? Name(N) Date(D) Time(T) Language(L) Visitors(V)");
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
                                    DateTime date = DateTime.MinValue;
                                    bool dateFormat = true;
                                    
                                    while (dateFormat)
                                    {
                                        Console.WriteLine("Date (Y-M-D H:M:S): ");
                                        string dateString = Console.ReadLine();
                                        
                                        if (!DateTime.TryParse(dateString, out date))
                                        {
                                            Console.WriteLine("Invalid date format. Please enter a valid date.\n");
                                        }
                                        else
                                        {
                                            dateFormat = false;
                                        }
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
        else
        {
            bool adminRunning = true;

            while (adminRunning)
            {
                Console.WriteLine("Overzicht rondleidingen(T)\nRondleiding toevoegen (A)\nrondleiding wijzigen (E)\nRondleiding verwijderen(R)\nQuit (Q)");
                string option = Console.ReadLine();
                
                if (option.ToLower() == "t")
                {
                    Tours.OverviewTours(true);
                }
                else if (option.ToLower() == "a")
                {
                    Console.WriteLine("Naam:");
                    string name = Console.ReadLine();
                    
                    DateTime date = DateTime.MinValue;
                    bool dateFormat = true;
                    
                    while (dateFormat)
                    {
                        Console.WriteLine("\nDatum (J-M-D U:M:S): ");
                        string dateString = Console.ReadLine();
                        
                        if (!DateTime.TryParse(dateString, out date))
                        {
                            Console.WriteLine("ongeldig data formaat. geef een juiste datum");
                        }
                        else
                        {
                            dateFormat = false;
                        }
                    }

                    Console.WriteLine("\nTaal: ");
                    string language = Console.ReadLine();

                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();

                        string insertTourDataCommand = @"
                            INSERT OR IGNORE INTO Tours (Name, Date, StartingPoint, EndPoint, Language, Visitors, Guide) VALUES 
                                (@Name, @Date, @StartingPoint, @EndPoint, @Language, @Visitors, @Guide);";

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
                    Console.WriteLine("Rondleiding (Id): ");
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

                                            table.AddRow(
                                                reader["Id"].ToString(),
                                                reader["Name"].ToString(),
                                                dateOnly,
                                                timeOnly,
                                                reader["StartingPoint"].ToString(),
                                                reader["EndPoint"].ToString(),
                                                reader["Language"].ToString(),
                                                reader["Guide"].ToString()
                                            );

                                            ctx.Refresh();
                                        }
                                    });
                                
                                Console.WriteLine("Wat wilt u aanpassen? Naam(N) Datum(D) Tijd(T) taal(L) bezoekers(V)");
                                string change = Console.ReadLine();

                                if (change.ToLower() == "n")
                                {
                                    Console.WriteLine("Naam:");
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
                                    DateTime date = DateTime.MinValue;
                                    bool dateFormat = true;
                                    
                                    while (dateFormat)
                                    {
                                        Console.WriteLine("Datum (Y-M-D H:M:S): ");
                                        string dateString = Console.ReadLine();
                                        
                                        if (!DateTime.TryParse(dateString, out date))
                                        {
                                            Console.WriteLine("ongeldig data formaat, voer een geldige datum in\n");
                                        }
                                        else
                                        {
                                            dateFormat = false;
                                        }
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
                                        Console.WriteLine($"Datum veranderd naar {date}");
                                    }
                                }
                                else if (change.ToLower() == "t")
                                {
                                    Console.WriteLine("Tijd (H:M:S): ");
                                    string timeString = Console.ReadLine();

                                    DateTime time;
                                    if (!DateTime.TryParseExact(timeString, "H:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                                    {
                                        Console.WriteLine("ongeldige tijd formaat. voer geldige tijd in (H:M:S).");
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
                                        Console.WriteLine($"Tijd veranderd naar {updatedDateTime.TimeOfDay}");
                                    }
                                }
                                else if (change.ToLower() == "l")
                                {
                                    Console.WriteLine("Taal: ");
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
                                        Console.WriteLine($"Taal veranderd naar {language}");
                                    }
                                }
                                else if (change.ToLower() == "v")
                                {
                                    Console.WriteLine("Bezoekers: ");
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
                                        Console.WriteLine($"bezoekers veranderd naar {visitors}");
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
                    Console.WriteLine("Rondleiding (Id): ");
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
                            Console.WriteLine("Rondleiding succesvol verwijderd");
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
    }
}