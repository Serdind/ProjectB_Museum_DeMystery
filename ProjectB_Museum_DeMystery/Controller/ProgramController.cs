public static class ProgramController
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

                bool accountCreated = Visitor.AccCreated(qr);

                string loginStatus;

                if (accountCreated)
                {
                    loginStatus = "Visitor";
                    visitor.QR = qr;
                }
                else
                {
                    loginStatus = Visitor.Login(qr);
                }

                if (loginStatus == "Visitor")
                {
                    bool visitorRunning = true;
                    while (visitorRunning)
                    {
                        string option = ReservationMenu.Menu();

                        if (option.ToLower() == "e")
                        {
                            TourController.ReservateTour(visitor);
                        }
                        else if (option.ToLower() == "m")
                        {
                            Visitor.ViewReservationsMade(qr);
                        }
                        else if (option.ToLower() == "c")
                        {
                            Visitor.CancelReservation(visitor);
                        }
                        else
                        {
                            WrongInput.Show();
                        }
                    }
                }
                else if (loginStatus == "Admin")
                {
                    PersonController.AdminMenu(language);
                }
                else if (loginStatus == "Guide")
                {
                    bool guideRunning = true;

                    while (guideRunning)
                    {
                        string option = GuideOptions.ViewTours();

                        if (option.ToLower() == "m")
                        {
                            Guide.ViewTours("Casper");
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