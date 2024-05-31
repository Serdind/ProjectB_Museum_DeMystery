using Spectre.Console;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class PersonController
{
    private static IMuseum museum = Program.Museum;

    public void AdminMenu()
    {
        string filePath = Model<GuidedTour>.GetFileNameTours();

        List<GuidedTour> tours = Tour.LoadToursFromFile();
        bool adminRunning = true;

        while (adminRunning)
        {
            string option = AdminOptions.Options();

            if (option.ToLower() == "t" || option.ToLower() == "overview tours")
            {
                Tour.OverviewTours();
            }
            else if (option.ToLower() == "a" || option.ToLower() == "add tour")
            {
                string language = "";
                bool tourAdded = false;

                AdminOptions.BackOption();

                while (true)
                {
                    string timeString = TourInfo.Time();
                    
                    if (timeString.ToLower() == "b" || timeString.ToLower() == "back")
                    {
                        break;
                    }

                    if (!DateTime.TryParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
                    {
                        TourInfo.InvalidTime();
                        continue;
                    }
                    
                    if (Tour.ToursExistForTime(time, tours))
                    {
                        TourInfo.ToursAlreadyExist();
                        continue;
                    }

                    language = TourInfo.Language();

                    if (language.ToLower() == "b" || language.ToLower() == "back")
                    {
                        break;
                    }

                    DateTime startDate = museum.Today.AddDays(1);
                    DateTime endDate = museum.Today.AddDays(7);

                    for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
                    {
                        GuidedTour newTour = new GuidedTour(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, time.Hour, time.Minute, 0), language, Tour.guide.Name);
                        Tour.AddTour(newTour, tours);
                    }

                    tourAdded = true;
                    break;
                }

                if (tourAdded)
                {
                    Tour.SaveToursToFile(filePath, tours);
                    MessageTourReservation.TourAdded();
                }
            }
            else if (option.ToLower() == "e" || option.ToLower() == "edit tour")
            {
                string newTimeInput;
                DateTime newTime;

                AdminOptions.BackOption();
                while (true)
                {
                    newTimeInput = EditTour.TimeEdit();

                    if (DateTime.TryParseExact(newTimeInput, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out newTime))
                    {
                        var toursWithSameTime = tours.Where(t => t.Date.TimeOfDay == newTime.TimeOfDay).ToList();

                        if (toursWithSameTime.Count > 0)
                        {
                            while (true)
                            {
                                DateTime currentDate = toursWithSameTime.First().Date.Date;
                                string change = EditTour.EditOptions();

                                if (change.ToLower() == "t" || change.ToLower() == "time")
                                {
                                    string newTimeInput1;
                                    DateTime newTime1;

                                    AdminOptions.BackOption();
                                    while (true)
                                    {
                                        newTimeInput1 = EditTour.NewTime();

                                        if (DateTime.TryParseExact(newTimeInput1, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out newTime1))
                                        {
                                            TimeSpan updatedTimeOfDay = newTime1.TimeOfDay;

                                            foreach (var otherTour in toursWithSameTime)
                                            {
                                                otherTour.Date = otherTour.Date.Date + updatedTimeOfDay;
                                            }

                                            Tour.SaveToursToFile(filePath, tours);

                                            EditTour.TimeSet(updatedTimeOfDay);
                                            break;
                                        }
                                        else if (newTimeInput1.ToLower() == "b" || newTimeInput1.ToLower() == "back")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            TourInfo.InvalidTime();
                                        }
                                    }
                                }
                                else if (change.ToLower() == "l" || change.ToLower() == "language")
                                {
                                    while (true)
                                    {
                                        string language = TourInfo.Language();

                                        if (language.ToLower() == "b" || language.ToLower() == "back")
                                        {
                                            break;
                                        }

                                        foreach (var otherTour in toursWithSameTime)
                                        {
                                            otherTour.Language = language;
                                        }

                                        Tour.SaveToursToFile(filePath, tours);

                                        EditTour.LanguageSet(language);
                                    }
                                }
                                else if (change.ToLower() == "g" || change.ToLower() == "guide")
                                {
                                    while (true)
                                    {
                                        string guide = TourInfo.Guide();

                                        if (guide.ToLower() == "b" || guide.ToLower() == "back")
                                        {
                                            break;
                                        }

                                        foreach (var otherTour in toursWithSameTime)
                                        {
                                            otherTour.NameGuide = guide;
                                        }

                                        Tour.SaveToursToFile(filePath, tours);

                                        EditTour.GuideSet(guide);
                                    }
                                }
                                else if (change.ToLower() == "s" || change.ToLower() == "status")
                                {
                                    bool newStatus = !toursWithSameTime.First().Status;

                                    foreach (var otherTour in toursWithSameTime)
                                    {
                                        otherTour.Status = newStatus;
                                    }

                                    Tour.SaveToursToFile(filePath, tours);

                                    EditTour.StatusSet(newStatus);
                                }
                                else if (change.ToLower() == "b" || change.ToLower() == "back")
                                {
                                    break;
                                }
                                else
                                {
                                    WrongInput.Show();
                                }
                            }
                        }
                        else
                        {
                            TourInfo.NoTours();
                        }
                    }
                    else if (newTimeInput.ToLower() == "b" || newTimeInput.ToLower() == "back")
                    {
                        break;
                    }
                    else
                    {
                        TourInfo.InvalidTime();
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
