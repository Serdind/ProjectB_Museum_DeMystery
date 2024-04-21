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

                    bool accountCreated = visitor.AccCreated(qr);

                    string loginStatus;

                    if (accountCreated)
                    {
                        loginStatus = "Visitor";
                        visitor.QR = qr;
                    }
                    else
                    {
                        loginStatus = visitor.Login(qr);
                    }

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
                        visitor.AdminMenu(2);
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
                                Tours.guide.ViewTours("Casper");
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