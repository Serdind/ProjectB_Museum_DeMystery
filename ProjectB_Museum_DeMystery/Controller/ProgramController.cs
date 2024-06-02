public class ProgramController
{
    public static void Start()
    {
        Visitor visitor = new Visitor(0, null);

        MainMenu.Welcome();

        string qr = QRVisitor.ScanQr();
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
            bool helpShown = false;

            if (!helpShown)
            {
                bool helpSucces = false;

                while (!helpSucces)
                {
                    string help = ReservationMenu.Help();

                    if (help.ToLower() == "b" || help.ToLower() == "back")
                    {
                        visitorRunning = false;
                    }
                    else if (help.ToLower() == "y" || help.ToLower() == "yes")
                    {
                        MainMenu.Intro();
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
                    TourController tourController = new TourController();
                    tourController.ReservateTour(visitor);
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
                            visitorRunning = false;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
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

            Tour.guide.ViewTours(guide.Name);
        }
    }
}