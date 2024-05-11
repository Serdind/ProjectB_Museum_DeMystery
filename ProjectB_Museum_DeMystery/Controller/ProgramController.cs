public class ProgramController
{
    public static void Start()
    {
        Visitor visitor = new Visitor(0,null);
        string language = MainMenu.Menu();

        if (language.ToLower() == "e")
        {
            string choice = LoginMenu.Login();

            if (choice.ToLower() == "l")
            {
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
                    while (visitorRunning)
                    {
                        string option = ReservationMenu.Menu(visitor.QR, visitor);

                        if (option.ToLower() == "e")
                        {
                            TourController tourController = new TourController();
                            tourController.ReservateTour(visitor);
                        }
                        else if (option.ToLower() == "m")
                        {
                            visitor.ViewReservationsMade(qr);
                        }
                        else if (option.ToLower() == "c")
                        {
                            visitor.CancelReservation(visitor);
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
                    personController.AdminMenu(language);
                }
                else if (loginStatus == "Guide")
                {
                    bool guideRunning = true;

                    while (guideRunning)
                    {
                        string option = GuideOptions.ViewTours();

                        if (option.ToLower() == "m")
                        {
                            Tour.guide.ViewTours("Casper");
                        }
                        else
                        {
                            WrongInput.Show();
                        }
                    }
                }
            }
            else
            {
                WrongInput.Show();
            }
        }
    }
}