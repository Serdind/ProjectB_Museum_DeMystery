using Spectre.Console;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class PersonController
{
    public void AdminMenu(string languageSelection)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        List<GuidedTour> tours = Tour.LoadToursFromFile();

        if (languageSelection.ToLower() == "e" || languageSelection.ToLower() == "english")
        {
            bool adminRunning = true;

            while (adminRunning)
            {
                string option = AdminOptions.Options();
                
                if (option.ToLower() == "t" || option.ToLower() == "overview tours")
                {
                    Tour.OverviewTours(true);
                }
                else if (option.ToLower() == "a" || option.ToLower() == "add tour")
                {
                    string name = "";
                    DateTime date = DateTime.MinValue;
                    string language = "";

                    bool tourAdded = false;

                    AdminOptions.BackOption();

                    while (true)
                    {
                        name = TourInfo.Name();
                    
                        if (name.ToLower() == "b" || name.ToLower() == "back")
                        {
                            break;
                        }

                        string dateString = TourInfo.Date();
                        
                        if (dateString.ToLower() == "b" || name.ToLower() == "back")
                        {
                            break;
                        }

                        if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            TourInfo.InvalidDate();
                            continue;
                        }

                        language = TourInfo.Language();

                        if (language.ToLower() == "b" || name.ToLower() == "back")
                        {
                            break;
                        }

                        tourAdded = true;
                        break;
                    }

                    if (tourAdded)
                    {
                        Tour.AddTour(new GuidedTour(name, date, language, Tour.guide.Name));
                    
                        Tour.SaveToursToFile(filePath, tours);

                        MessageTourReservation.TourAdded();
                    }
                }
                else if (option.ToLower() == "e" || option.ToLower() == "edit tour")
                {
                    Tour.OverviewTours(true);
                    int id = TourId.WhichTourId();

                    GuidedTour tour = tours.FirstOrDefault(v => v.ID == id);

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
                                table.AddColumn("Status");
                                
                                DateTime dateValue = Convert.ToDateTime(tour.Date);
                                string timeOnly = dateValue.ToString("HH:mm");
                                string dateOnly = dateValue.ToShortDateString();
                                string status = tour.Status ? "Active" : "Inactive";

                                table.AddRow(
                                    tour.Name.ToString(),
                                    dateOnly,
                                    timeOnly,
                                    tour.Language.ToString(),
                                    tour.NameGuide.ToString(),
                                    status
                                );

                                ctx.Refresh();
                            });
                        
                        string change = EditTour.EditOptions();
                        
                        if (change.ToLower() == "n" || change.ToLower() == "name")
                        {
                            string name = TourInfo.Name();

                            tour.Name = name;
                            EditTour.NameSet(name);
                            Tour.SaveToursToFile(filePath, tours);
                            
                        }
                        else if (change.ToLower() == "d" || change.ToLower() == "date")
                        {
                            DateTime date = DateTime.MinValue;
                            bool dateFormat = true;
                            
                            while (dateFormat)
                            {
                                string dateString = TourInfo.Date();
                                
                                if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                {
                                    TourInfo.InvalidDate();
                                    continue;
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
                        else if (change.ToLower() == "t" || change.ToLower() == "time")
                        {
                            string timeString = TourInfo.Time();

                            DateTime time;
                            if (!DateTime.TryParseExact(timeString, "H:m", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
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
                        else if (change.ToLower() == "l" || change.ToLower() == "language")
                        {
                            string language = TourInfo.Language();

                            tour.Language = language;

                            EditTour.LanguageSet(language);
                            Tour.SaveToursToFile(filePath, tours);
                        }

                        else if (change.ToLower() == "g" || change.ToLower() == "guide")
                        {
                            string guide = TourInfo.Guide();

                            tour.NameGuide = guide;

                            EditTour.GuideSet(guide);
                            Tour.SaveToursToFile(filePath, tours);
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
                            EditTour.StatusSet(tour.Status);
                            Tour.SaveToursToFile(filePath, tours);
                            Tour.OverviewTours(true);
                        }
                        else if (change.ToLower() == "b" || change.ToLower() == "go back")
                        {
                            break;
                        }
                        else
                        {
                            WrongInput.Show();
                        }
                    }
                }
                else if (option.ToLower() == "l" || option.ToLower() == "log out")
                {
                    adminRunning = false;
                }
                else
                {
                    WrongInput.Show();
                }
            }
        }
    }
}
