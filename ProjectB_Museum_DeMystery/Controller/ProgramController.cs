public class ProgramController
{
    public static void Start()
    {
        if (!Tour.OverviewTours(false))
        {
            Environment.Exit(1);
        }

        Visitor visitor = new Visitor(0, null);

        MainMenu.Welcome();

        string qr;
        bool accountCreated = false;
        string loginStatus = "";

        while (true)
        {
            qr = QRVisitor.ScanQr();
            accountCreated = visitor.AccCreated(qr);

            if (qr.ToLower() == "b" || qr.ToLower() == "back")
            {
                break;
            }

            if (accountCreated)
            {
                loginStatus = "Visitor";
                visitor.QR = qr;
                break;
            }
            else
            {
                loginStatus = visitor.Login(qr);
                if (loginStatus == "Visitor" || loginStatus == "Admin" || loginStatus == "Guide")
                {
                    break;
                }
            }
        }

        if (loginStatus == "Visitor")
        {
            bool visitorRunning = true;
            bool helpShown = false;

            LoggedIn.VisitorLoginMessageEn(visitor);

            if (!helpShown)
            {
                bool helpSucces = false;

                while (!helpSucces)
                {
                    string help = ReservationMenu.Help();

                    if (help.ToLower() == "b" || help.ToLower() == "back")
                    {
                        helpSucces = true;
                        visitorRunning = false;
                    }
                    else if (help.ToLower() == "y" || help.ToLower() == "yes")
                    {
                        MainMenu.Intro();
                        ReservationMenu.HelpActive();
                        helpShown = true;
                        helpSucces = true;
                    }
                    else if (help.ToLower() == "n" || help.ToLower() == "no")
                    {
                        ReservationMenu.HelpActive();
                        helpSucces = true;
                    }
                    else
                    {
                        WrongInput.Show();
                    }
                }
            }

            while (visitorRunning)
            {
                string option = ReservationMenu.Menu(visitor.QR, visitor);

                if (option.ToLower() == "r" || option.ToLower() == "make reservation")
                {
                    if (!visitor.ReservationMade(visitor.QR))
                    {
                        TourController tourController = new TourController();
                        tourController.ReservateTour(visitor);
                    }
                    else
                    {
                        MaxReservation.Show();
                    }
                }
                else if (option.ToLower() == "m" || option.ToLower() == "my reservations")
                {
                    visitor.ViewReservationsMade(qr);
                }
                else if (option.ToLower() == "c" || option.ToLower() == "cancel reservation")
                {
                    visitor.CancelReservation(visitor);
                }
                else if (option.ToLower() == "h" || option.ToLower() == "help")
                {
                    MainMenu.Intro();
                }
                else if (option.ToLower() == "f" || option.ToLower() == "finish")
                {
                    if (!visitor.ReservationMade(qr))
                    {
                        string finish = ReservationMenu.Finish();

                        if (finish.ToLower() == "y" || finish.ToLower() == "yes")
                        {
                            MainMenu.Goodbye();
                            visitorRunning = false;
                        }
                        else
                        {
                            Console.Clear();
                            continue;
                        }
                    }
                    else
                    {
                        MainMenu.Goodbye();
                        visitorRunning = false;
                    }
                }
                else
                {
                    WrongInput.Show();
                }
            }
        }
        else if (loginStatus == "Admin")
        {
            PersonController personController = new PersonController();
            personController.AdminMenu();
        }
        else if (loginStatus == "Guide")
        {
            List<Guide> guides = Tour.LoadGuidesFromFile();

            Guide guide = guides.FirstOrDefault(v => v.QR == qr);

            guide.ViewTours(guide.Name, guide);
        }
    }
}