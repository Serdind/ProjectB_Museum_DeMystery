using Spectre.Console;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public static class PersonController
{
    public static void AdminMenu(string languageSelection)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName1 = "removedTours.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

        List<GuidedTour> tours = Tour.LoadToursFromFile();

        if (languageSelection.ToLower() == "e")
        {
            bool adminRunning = true;

            while (adminRunning)
            {
                string option = AdminOptions.Options();
                
                if (option.ToLower() == "t")
                {
                    Tour.OverviewTours(true);
                }
                else if (option.ToLower() == "a")
                {
                    string name = TourInfo.Name();
                    
                    DateTime date = DateTime.MinValue;
                    bool dateFormat = true;
                    
                    while (dateFormat)
                    {
                        string dateString = TourInfo.Date();
                        
                        if (!DateTime.TryParse(dateString, out date))
                        {
                            TourInfo.InvalidDate();
                        }
                        else
                        {
                            dateFormat = false;
                        }
                    }

                    string language = TourInfo.Language();

                    Tour.AddTour(new GuidedTour(name, date, language, Tour.guide.Name));
                    
                    Tour.SaveToursToFile(filePath, tours);
                }
                else if (option.ToLower() == "e")
                {
                    Tour.OverviewTours(true);
                    int id = TourId.WhichTourId();

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
                                    tour.Language.ToString(),
                                    tour.NameGuide.ToString()
                                );

                                ctx.Refresh();
                            });
                        
                        string change = EditTour.EditOptions();
                        
                        if (change.ToLower() == "n")
                        {
                            string name = TourInfo.Name();

                            tour.Name = name;
                            EditTour.NameSet(name);
                            Tour.SaveToursToFile(filePath, tours);
                            
                        }
                        else if (change.ToLower() == "d")
                        {
                            DateTime date = DateTime.MinValue;
                            bool dateFormat = true;
                            
                            while (dateFormat)
                            {
                                string dateString = TourInfo.Date();
                                
                                if (!DateTime.TryParse(dateString, out date))
                                {
                                    TourInfo.InvalidDate();
                                }
                                else
                                {
                                    dateFormat = false;
                                }
                            }

                            date = date.Date;

                            tour.Date = date;
                            
                            EditTour.DateSet(date);
                            Tour.SaveToursToFile(filePath, tours);
                            
                        }
                        else if (change.ToLower() == "t")
                        {
                            string timeString = TourInfo.Time();

                            DateTime time;
                            if (!DateTime.TryParseExact(timeString, "H:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                            {
                                TourInfo.InvalidTime();
                                return;
                            }

                            DateTime currentDate = tour.Date.Date;

                            DateTime updatedDateTime = currentDate.Add(time.TimeOfDay);

                            tour.Date = updatedDateTime;

                            EditTour.TimeSet(updatedDateTime.TimeOfDay);
                            Tour.SaveToursToFile(filePath, tours);
                        }
                        else if (change.ToLower() == "l")
                        {
                            string language = TourInfo.Language();

                            tour.Language = language;

                            EditTour.LanguageSet(language);
                            Tour.SaveToursToFile(filePath, tours);
                        }

                        else if (change.ToLower() == "g")
                        {
                            string guide = TourInfo.Guide();

                            tour.NameGuide = guide;

                            EditTour.GuideSet(guide);
                            Tour.SaveToursToFile(filePath, tours);
                        }
                        else
                        {
                            WrongInput.Show();
                        }
                    }
                }
                else if (option.ToLower() == "r")
                {
                    Tour.OverviewTours(true);
                    int id = TourId.WhichTourId();

                    GuidedTour tour = tours.FirstOrDefault(v => v.ID == id);

                    if (tour != null)
                    {
                        string tourJson = JsonConvert.SerializeObject(tour, Formatting.Indented);

                        File.WriteAllText(filePath1, $"[{tourJson}]");

                        tours.Remove(tour);

                        string updatedJsonData = JsonConvert.SerializeObject(tours, Formatting.Indented);
                        File.WriteAllText(filePath, updatedJsonData);

                        TourInfo.TourRemoved();
                    }
                    else
                    {
                        TourNotFound.Show();
                    }
                }
                else if (option.ToLower() == "s")
                {
                    Tour.OverviewRemovedTours();

                    int id = TourId.WhichTourId();

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

                        TourInfo.TourRestored();
                    }
                    else
                    {
                        TourNotFound.Show();
                    }
                }
                else if (option.ToLower() == "q")
                {
                    adminRunning = false;
                    continue;
                }
                else
                {
                    WrongInput.Show();
                }
            }
        }
    }
}