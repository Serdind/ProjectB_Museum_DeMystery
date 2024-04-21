using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;
using Newtonsoft.Json;

class Person
{

    public string QR;
    public int Id;

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

    public void AdminMenu(string languageSelection)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName1 = "removedTours.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

        List<GuidedTour> tours = Tours.LoadToursFromFile();

        if (languageSelection.ToLower() == "e")
        {
            bool adminRunning = true;

            while (adminRunning)
            {
                Console.WriteLine("Overview tours(T)\nAdd tour (A)\nEdit tour (E)\nRemove tour (R)\nRestore tour (S)\nQuit (Q)");
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

                    Tours.AddTour(new GuidedTour(name, date, language, Tours.guide.Name));
                    
                    Tours.SaveToursToFile(filePath, tours);
                }
                else if (option.ToLower() == "e")
                {
                    Tours.OverviewTours(true);
                    Console.WriteLine("Tour (Id): ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    int actualID = id + 9;

                    GuidedTour tour = tours.FirstOrDefault(v => v.ID == actualID);

                    if (tour != null)
                    {
                        var table = new Table().LeftAligned();

                        AnsiConsole.Live(table)
                            .AutoClear(false)
                            .Overflow(VerticalOverflow.Ellipsis)
                            .Cropping(VerticalOverflowCropping.Top)
                            .Start(ctx =>
                            {
                                table.AddColumn("Name");
                                table.AddColumn("Date");
                                table.AddColumn("Time");
                                table.AddColumn("Language");
                                table.AddColumn("Guide");
                                DateTime dateValue = Convert.ToDateTime(tour.Date);
                                string timeOnly = dateValue.ToString("HH:mm");
                                string dateOnly = dateValue.ToShortDateString();

                                table.AddRow(
                                    tour.Name.ToString(),
                                    dateOnly,
                                    timeOnly,
                                    GuidedTour.StartingPoint.ToString(),
                                    GuidedTour.EndPoint.ToString(),
                                    tour.Language.ToString(),
                                    tour.NameGuide.ToString()
                                );

                                ctx.Refresh();
                            });
                        
                        Console.WriteLine("What do you want to change? Name(N) Date(D) Time(T) Language(L) Guide(G)");
                        string change = Console.ReadLine();

                        if (change.ToLower() == "n")
                        {
                            Console.WriteLine("Name:");
                            string name = Console.ReadLine();

                            tour.Name = name;
                            Console.WriteLine($"Name set to {name}");
                            Tours.SaveToursToFile(filePath, tours);
                            
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

                            tour.Date = date;
                            
                            Console.WriteLine($"Date set to {date}");
                            Tours.SaveToursToFile(filePath, tours);
                            
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

                            DateTime currentDate = tour.Date.Date;

                            DateTime updatedDateTime = currentDate.Add(time.TimeOfDay);

                            tour.Date = updatedDateTime;

                            Console.WriteLine($"Time set to {updatedDateTime.TimeOfDay}");
                            Tours.SaveToursToFile(filePath, tours);
                        }
                        else if (change.ToLower() == "l")
                        {
                            Console.WriteLine("Language: ");
                            string language = Console.ReadLine();

                            tour.Language = language;

                            Console.WriteLine($"Language set to {language}");
                            Tours.SaveToursToFile(filePath, tours);
                        }

                        else if (change.ToLower() == "g")
                        {
                            Console.WriteLine("Guide: ");
                            string guide = Console.ReadLine();

                            tour.NameGuide = guide;

                            Console.WriteLine($"Guide set to {guide}");
                            Tours.SaveToursToFile(filePath, tours);
                        }
                        else
                        {
                            Console.WriteLine("Wrong input. Try again.");
                        }
                    }
                }
                else if (option.ToLower() == "r")
                {
                    Tours.OverviewTours(true);
                    Console.WriteLine("Tour (Id): ");
                    int id;
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid ID.");
                        continue;
                    }

                    GuidedTour tour = tours.FirstOrDefault(v => v.ID == id + 9);

                    if (tour != null)
                    {
                        string tourJson = JsonConvert.SerializeObject(tour, Formatting.Indented);

                        File.WriteAllText(filePath1, $"[{tourJson}]");

                        tours.Remove(tour);

                        string updatedJsonData = JsonConvert.SerializeObject(tours, Formatting.Indented);
                        File.WriteAllText(filePath, updatedJsonData);

                        Console.WriteLine("Tour removed successfully.");
                        Console.WriteLine("Removed tour details:");
                    }
                    else
                    {
                        Console.WriteLine("Tour not found.");
                    }
                }
                else if (option.ToLower() == "s")
                {
                    Tours.OverviewRemovedTours();
                    Console.WriteLine("Tour (Id): ");
                    int id;
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid ID.");
                        continue;
                    }

                    string jsonRemovedTours = File.ReadAllText(filePath1);

                    List<GuidedTour> removedTours = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonRemovedTours);

                    GuidedTour removedTour = removedTours.FirstOrDefault(v => v.ID == id);

                    if (removedTour != null)
                    {
                        tours.Add(removedTour);

                        string serializedTours = JsonConvert.SerializeObject(tours, Formatting.Indented);
                        File.WriteAllText(filePath, serializedTours);

                        removedTours.Remove(removedTour);
                        string updatedRemovedTours = JsonConvert.SerializeObject(removedTours, Formatting.Indented);
                        File.WriteAllText(filePath1, updatedRemovedTours);

                        Console.WriteLine("Tour successfully restored.");
                    }
                    else
                    {
                        Console.WriteLine("Tour not found in removed tours.");
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
        else if (languageSelection.ToLower() == "n")
        {
            bool adminRunning = true;

            while (adminRunning)
            {
                Console.WriteLine("Overzicht rondleidingen(T)\nToevoeging rondleiding (A)\nBewerk rondleiding (E)\nVerwijder rondleiding (R)\nHerstel rondleiding (S)\nAfsluiten (Q)");
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
                            Console.WriteLine("Ongeldige datum. Gelieve een correcte datum in te voeren.");
                        }
                        else
                        {
                            dateFormat = false;
                        }
                    }

                    Console.WriteLine("\nTaal: ");
                    string language = Console.ReadLine();

                    Tours.AddTour(new GuidedTour(name, date, language, Tours.guide.Name));
                    
                    Tours.SaveToursToFile(filePath, tours);
                }
                else if (option.ToLower() == "e")
                {
                    Tours.OverviewTours(true);
                    Console.WriteLine("Rondleiding (Id): ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    int actualID = id + 9;

                    GuidedTour tour = tours.FirstOrDefault(v => v.ID == actualID);

                    if (tour != null)
                    {
                        var table = new Table().LeftAligned();

                        AnsiConsole.Live(table)
                            .AutoClear(false)
                            .Overflow(VerticalOverflow.Ellipsis)
                            .Cropping(VerticalOverflowCropping.Top)
                            .Start(ctx =>
                            {
                                table.AddColumn("Naam");
                                table.AddColumn("Datum");
                                table.AddColumn("Tijd");
                                table.AddColumn("Taal");
                                table.AddColumn("Gids");
                                DateTime dateValue = Convert.ToDateTime(tour.Date);
                                string timeOnly = dateValue.ToString("HH:mm");
                                string dateOnly = dateValue.ToShortDateString();

                                table.AddRow(
                                    tour.Name.ToString(),
                                    dateOnly,
                                    timeOnly,
                                    GuidedTour.StartingPoint.ToString(),
                                    GuidedTour.EndPoint.ToString(),
                                    tour.Language.ToString(),
                                    tour.NameGuide.ToString()
                                );

                                ctx.Refresh();
                            });
                        
                        Console.WriteLine("Wat wil je veranderen? Naam(N) Datum(D) Tijd(T) Taal(L) Gids(G)");
                        string change = Console.ReadLine();

                        if (change.ToLower() == "n")
                        {
                            Console.WriteLine("Naam:");
                            string name = Console.ReadLine();

                            tour.Name = name;
                            Console.WriteLine($"Naam ingesteld op {name}");
                            Tours.SaveToursToFile(filePath, tours);
                            
                        }
                        else if (change.ToLower() == "d")
                        {
                            DateTime date = DateTime.MinValue;
                            bool dateFormat = true;
                            
                            while (dateFormat)
                            {
                                Console.WriteLine("Datum (J-M-D U:M:S): ");
                                string dateString = Console.ReadLine();
                                
                                if (!DateTime.TryParse(dateString, out date))
                                {
                                    Console.WriteLine("Ongeldige datum. Gelieve een correcte datum in te voeren.\n");
                                }
                                else
                                {
                                    dateFormat = false;
                                }
                            }

                            date = date.Date;

                            tour.Date = date;
                            
                            Console.WriteLine($"Datum ingesteld op {date}");
                            Tours.SaveToursToFile(filePath, tours);
                            
                        }
                        else if (change.ToLower() == "t")
                        {
                            Console.WriteLine("Tijd (U:M:S): ");
                            string timeString = Console.ReadLine();

                            DateTime time;
                            if (!DateTime.TryParseExact(timeString, "H:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                            {
                                Console.WriteLine("Ongeldige tijdnotatie. Voer een geldige tijd in (U:M:S).");
                                return;
                            }

                            DateTime currentDate = tour.Date.Date;

                            DateTime updatedDateTime = currentDate.Add(time.TimeOfDay);

                            tour.Date = updatedDateTime;

                            Console.WriteLine($"Tijd ingesteld op {updatedDateTime.TimeOfDay}");
                            Tours.SaveToursToFile(filePath, tours);
                        }
                        else if (change.ToLower() == "l")
                        {
                            Console.WriteLine("Taal: ");
                            string language = Console.ReadLine();

                            tour.Language = language;

                            Console.WriteLine($"Taal ingesteld op {language}");
                            Tours.SaveToursToFile(filePath, tours);
                        }

                        else if (change.ToLower() == "g")
                        {
                            Console.WriteLine("Gids: ");
                            string guide = Console.ReadLine();

                            tour.NameGuide = guide;

                            Console.WriteLine($"Gids ingesteld op {guide}");
                            Tours.SaveToursToFile(filePath, tours);
                        }
                        else
                        {
                            Console.WriteLine("Verkeerde invoer. Probeer het nog eens.");
                        }
                    }
                }
                else if (option.ToLower() == "r")
                {
                    Tours.OverviewTours(true);
                    Console.WriteLine("Rondleiding (Id): ");
                    int id;
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        Console.WriteLine("Ongeldige invoer. Voer een geldig ID in.");
                        continue;
                    }

                    GuidedTour tour = tours.FirstOrDefault(v => v.ID == id + 9);

                    if (tour != null)
                    {
                        string tourJson = JsonConvert.SerializeObject(tour, Formatting.Indented);

                        File.WriteAllText(filePath1, $"[{tourJson}]");

                        tours.Remove(tour);

                        string updatedJsonData = JsonConvert.SerializeObject(tours, Formatting.Indented);
                        File.WriteAllText(filePath, updatedJsonData);

                        Console.WriteLine("Rondleiding succesvol verwijderd.");
                    }
                    else
                    {
                        Console.WriteLine("Rondleiding niet gevonden.");
                    }
                }
                else if (option.ToLower() == "s")
                {
                    Tours.OverviewRemovedTours();
                    Console.WriteLine("Rondleiding (Id): ");
                    int id;
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        Console.WriteLine("Ongeldige invoer. Voer een geldig ID in.");
                        continue;
                    }

                    string jsonRemovedTours = File.ReadAllText(filePath1);

                    List<GuidedTour> removedTours = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonRemovedTours);

                    GuidedTour removedTour = removedTours.FirstOrDefault(v => v.ID == id);

                    if (removedTour != null)
                    {
                        tours.Add(removedTour);

                        string serializedTours = JsonConvert.SerializeObject(tours, Formatting.Indented);
                        File.WriteAllText(filePath, serializedTours);

                        removedTours.Remove(removedTour);
                        string updatedRemovedTours = JsonConvert.SerializeObject(removedTours, Formatting.Indented);
                        File.WriteAllText(filePath1, updatedRemovedTours);

                        Console.WriteLine("Tour succesvol hersteld.");
                    }
                    else
                    {
                        Console.WriteLine("Tour niet gevonden in verwijderde tours.");
                    }
                }
                else if (option.ToLower() == "q")
                {
                    adminRunning = false;
                    continue;
                }
                else
                {
                    Console.WriteLine("Verkeerde invoer. Probeer het nog eens.");
                }
            }
        }
    }
}