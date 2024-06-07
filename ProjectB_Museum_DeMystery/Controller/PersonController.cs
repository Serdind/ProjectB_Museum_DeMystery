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
                if (Tour.OverviewTours(true))
                {
                    AdminOptions.PressAnything();
                }
            }
            else if (option.ToLower() == "a" || option.ToLower() == "add tour")
            {
                Console.Clear();
                AdminOptions.BackOption();
                string timeString = TourInfo.Time();

                if (timeString.ToLower() == "b" || timeString.ToLower() == "back")
                {
                    continue;
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

                string language = TourInfo.Language();

                if (language.ToLower() == "b" || language.ToLower() == "back")
                {
                    continue;
                }

                if (string.IsNullOrEmpty(language))
                {
                    AdminOptions.Empty();
                    continue;
                }

                string confirm = AdminOptions.Confirm();

                if (confirm.ToLower() == "yes" || confirm.ToLower() == "y")
                {
                    DateTime startDate = museum.Today.AddDays(1);

                    if (!Tour.ToursExistForTimeAndLanguage(new DateTime(startDate.Year, startDate.Month, startDate.Day, time.Hour, time.Minute, 0), language, tours))
                    {
                        GuidedTour newTour = new GuidedTour(new DateTime(startDate.Year, startDate.Month, startDate.Day, time.Hour, time.Minute, 0), language, "Casper");
                        Tour.AddTour(newTour, tours);
                        Tour.SaveToursToFile(filePath, tours);
                        MessageTourReservation.TourAdded();
                        Tour.OverviewToursTomorrow();
                        AdminOptions.PressAnything();
                    }
                }
                else if (confirm.ToLower() == "no" || confirm.ToLower() == "n")
                {
                    continue;
                }
                else
                {
                    WrongInput.Show();
                }
            }
            else if (option.ToLower() == "e" || option.ToLower() == "edit tour")
            {
                string newTimeInput;
                DateTime newTime;

                while (true)
                {
                    Console.Clear();
                    AdminOptions.BackOption();
                    Tour.OverviewToursEdit();
                    newTimeInput = EditTour.TimeEdit();

                    if (DateTime.TryParseExact(newTimeInput, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out newTime))
                    {
                        var toursWithSameTime = tours.Where(t => t.Date.TimeOfDay == newTime.TimeOfDay).ToList();

                        if (toursWithSameTime.Count > 0)
                        {
                            while (true)
                            {
                                DateTime currentDate = toursWithSameTime.First().Date.Date;
                                DateTime selectedDate = DateTime.Today.AddDays(1);
                                string currentSelectedTime = newTimeInput;
                                string newTimeInput1 = newTimeInput;
                                bool timeChanged = false;

                                Console.Clear();
                                Tour.SelectedTour(newTimeInput, selectedDate);

                                string change = EditTour.EditOptions();

                                if (change.ToLower() == "t" || change.ToLower() == "time")
                                {
                                    Console.Clear();
                                    DateTime newTime1;

                                    while (true)
                                    {
                                        Console.Clear();
                                        AdminOptions.BackOption();
                                        Tour.SelectedTour(newTimeInput, selectedDate);
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
                                            timeChanged = true;
                                            newTimeInput = newTimeInput1;
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
                                        Console.Clear();
                                        AdminOptions.BackOption();
                                        Tour.SelectedTour(newTimeInput, selectedDate);
                                        string language = TourInfo.Language();

                                        if (language.ToLower() == "b" || language.ToLower() == "back")
                                        {
                                            break;
                                        }

                                        if (language.ToLower() == string.Empty)
                                        {
                                            AdminOptions.Empty();
                                            continue;
                                        }

                                        foreach (var otherTour in toursWithSameTime)
                                        {
                                            otherTour.Language = language;
                                        }

                                        Tour.SaveToursToFile(filePath, tours);

                                        EditTour.LanguageSet(language);
                                        break;
                                    }
                                }
                                else if (change.ToLower() == "s" || change.ToLower() == "status")
                                {
                                    while (true)
                                    {
                                        Console.Clear();
                                        Tour.SelectedTour(newTimeInput, selectedDate);
                                        bool newStatus = !toursWithSameTime.First().Status;

                                        foreach (var otherTour in toursWithSameTime)
                                        {
                                            otherTour.Status = newStatus;
                                        }

                                        Tour.SaveToursToFile(filePath, tours);

                                        EditTour.StatusSet(newStatus);
                                        break;
                                    }
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
                            TourInfo.NoToursTime();
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
