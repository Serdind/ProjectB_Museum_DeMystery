using Spectre.Console;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class TestablePersonController
{
    public readonly IMuseum Museum;

    public TestablePersonController(IMuseum museum)
    {
        Museum = museum;
    }

    public void AdminMenu(string languageSelection)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        List<GuidedTour> tours = TestJsonData.LoadToursFromTestFile(Museum);

        if (languageSelection.ToLower() == "e" || languageSelection.ToLower() == "english")
        {
            bool adminRunning = true;
            TestableTour testableTour = new TestableTour(Museum);

            while (adminRunning)
            {
                Museum.WriteLine("Overview tours(T)\nAdd tour (A)\nEdit tour (E)\nLog out (L)");
                string option = Museum.ReadLine();
                
                if (option.ToLower() == "t" || option.ToLower() == "overview tours")
                {
                    testableTour.OverviewTours(true);
                }
                else if (option.ToLower() == "a" || option.ToLower() == "add tour")
                {
                    string name = "";
                    DateTime date = Museum.MinValue;
                    string language = "";

                    bool tourAdded = false;

                    Museum.WriteLine("Insert (Back or B) if you want to go back");

                    while (true)
                    {
                        Museum.WriteLine("Name:");
                        name = Museum.ReadLine();
                    
                        if (name.ToLower() == "b" || name.ToLower() == "back")
                        {
                            break;
                        }

                        Museum.WriteLine("\nDate (Day-Month-Year Hour:Minutes): ");
                        string dateString = Museum.ReadLine();
                        
                        if (dateString.ToLower() == "b" || name.ToLower() == "back")
                        {
                            break;
                        }

                        if (!DateTime.TryParseExact(dateString, "d-M-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            Console.WriteLine("Invalid date format. Please enter a valid date.");
                            continue;
                        }

                        Museum.WriteLine("\nLanguage: ");
                        language = Museum.ReadLine();

                        if (language.ToLower() == "b" || name.ToLower() == "back")
                        {
                            break;
                        }

                        tourAdded = true;
                        break;
                    }

                    if (tourAdded)
                    {
                        testableTour.AddTour(new GuidedTour(name, date, language, "TestGuide"));
                    
                        testableTour.SaveToursToFile(filePath, tours);

                        Museum.WriteLine("Tour succesfully added.");
                    }
                }
                else if (option.ToLower() == "e" || option.ToLower() == "edit tour")
                {
                    bool toursFound = testableTour.OverviewTours(true);

                    if (toursFound)
                    {
                        int id = TourId.WhichTourId();

                        GuidedTour tour = tours.FirstOrDefault(v => v.ID == id);

                        if (tour != null)
                        {
                            var table = new Table().LeftAligned();

                            table.AddColumn("ID");
                            table.AddColumn("Name");
                            table.AddColumn("Date");
                            table.AddColumn("Time");
                            table.AddColumn("Duration");
                            table.AddColumn("Language");
                            table.AddColumn("Guide");
                            table.AddColumn("Remaining Spots");
                            table.AddColumn("Status");
                            
                            string timeOnly = tour.Date.ToString("HH:mm");
                            string dateOnly = tour.Date.ToShortDateString();
                            int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                            string status = tour.Status ? "Active" : "Inactive";

                            table.AddRow(
                                tour.ID.ToString(),
                                tour.Name,
                                dateOnly,
                                timeOnly,
                                "40 minutes",
                                tour.Language,
                                tour.NameGuide,
                                remainingSpots.ToString(),
                                status
                            );

                            Museum.WriteLine(table.ToString());
                            
                            Museum.WriteLine("What do you want to change? Name(N) Date(D) Time(T) Language(L) Guide(G) Status(S) Go back(B)");
                            string change = Museum.ReadLine();
                            
                            if (change.ToLower() == "n" || change.ToLower() == "name")
                            {
                                Museum.WriteLine("Name:");
                                string name = Museum.ReadLine();

                                tour.Name = name;
                                Museum.WriteLine($"Name set to {name}");
                                testableTour.SaveToursToFile(filePath, tours);
                                
                            }
                            else if (change.ToLower() == "d" || change.ToLower() == "date")
                            {
                                bool validDateSelected = false;
                                DateTime selectedDate = Museum.MinValue;

                                while (!validDateSelected)
                                {
                                    Museum.WriteLine("\nDate (Day-Month-Year Hour:Minutes): ");
                                    string dateString = Museum.ReadLine();

                                    if (dateString.ToLower() == "b" || dateString.ToLower() == "back")
                                    {
                                        break;
                                    }

                                    if (DateTime.TryParseExact(dateString, "d-M-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate))
                                    {
                                        validDateSelected = true;
                                    }
                                    else
                                    {
                                        Museum.WriteLine("Invalid date format. Please enter a valid date.");
                                    }
                                }

                                if (validDateSelected)
                                {
                                    tour.Date = selectedDate;
                                    Museum.WriteLine($"Date set to {selectedDate}");
                                    testableTour.SaveToursToFile(filePath, tours);
                                }
                            }
                            else if (change.ToLower() == "t" || change.ToLower() == "time")
                            {
                                Museum.WriteLine("Time (Hour:Minutes):");
                                string timeString = Museum.ReadLine();

                                DateTime time;
                                if (!DateTime.TryParseExact(timeString, "H:m", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                                {
                                    Museum.WriteLine("Invalid time format. Please enter a valid time (Hour:Minutes).");
                                    return;
                                }

                                DateTime currentDate = tour.Date.Date;

                                DateTime updatedDateTime = currentDate.Add(time.TimeOfDay);

                                tour.Date = updatedDateTime;

                                Museum.WriteLine($"Time set to {updatedDateTime.TimeOfDay}");
                                testableTour.SaveToursToFile(filePath, tours);
                            }
                            else if (change.ToLower() == "l" || change.ToLower() == "language")
                            {
                                Museum.WriteLine("\nLanguage: ");
                                string language = Museum.ReadLine();

                                tour.Language = language;

                                Museum.WriteLine($"Language set to {language}");
                                testableTour.SaveToursToFile(filePath, tours);
                            }

                            else if (change.ToLower() == "g" || change.ToLower() == "guide")
                            {
                                Museum.WriteLine("Guide:");
                                string guide = Museum.ReadLine();

                                tour.NameGuide = guide;

                                Museum.WriteLine($"Guide set to {guide}");
                                testableTour.SaveToursToFile(filePath, tours);
                            }
                            else if (change.ToLower() == "s" || change.ToLower() == "status")
                            {
                                if (tour.Status == true)
                                {
                                    tour.Status = false;
                                }
                                else
                                {
                                    tour.Status = true;
                                }

                                if (tour.Status == true)
                                {
                                    Museum.WriteLine($"Status set to active");
                                }
                                else
                                {
                                    Museum.WriteLine($"Status set to inactive");
                                }

                                testableTour.SaveToursToFile(filePath, tours);
                                testableTour.OverviewTours(true);
                            }
                            else if (change.ToLower() == "b" || change.ToLower() == "go back")
                            {
                                
                            }
                            else
                            {
                                Museum.WriteLine("Wrong input. Try again.");
                            }
                        }
                    }
                    else
                    {
                        
                    }
                }
                else if (option.ToLower() == "l" || option.ToLower() == "log out")
                {
                    adminRunning = false;
                }
                else
                {
                    Museum.WriteLine("Wrong input. Try again.");
                }
            }
        }
    }
}
