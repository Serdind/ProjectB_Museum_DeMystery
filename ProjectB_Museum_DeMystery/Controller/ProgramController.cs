public class ProgramController
{
    private static IMuseum museum = Program.Museum;
    public static void Start()
    {
        Visitor visitor = new Visitor(0, null);
        bool showIntro = true;

        string language;
        do
        {
            if (showIntro)
            {
                MainMenu.Intro();
                showIntro = false;
            }

            language = MainMenu.Language();

            if (!IsValidLanguage(language))
            {
                museum.WriteLine("Invalid language selection. Please choose again.");
            }

        } while (!IsValidLanguage(language));

        language = language.ToLower();

        string choice;
        bool showLoginPrompt = true;

        do
        {
            if (showLoginPrompt)
            {
                choice = LoginMenu.Login();
                showLoginPrompt = false;
            }
            else
            {
                choice = LoginMenu.Login();
            }

            if (choice.ToLower() != "l" && choice.ToLower() != "login")
            {
                museum.WriteLine("Invalid choice. Please try again.");
            }

        } while (choice.ToLower() != "l" && choice.ToLower() != "login");

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
                else if (option.ToLower() == "l" || option.ToLower() == "log out")
                {
                    visitorRunning = false;
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

                if (option.ToLower() == "m" || option.ToLower() == "my tours")
                {
                    Tour.guide.ViewTours("Casper");
                }
                else if (option.ToLower() == "l" || option.ToLower() == "log out")
                {
                    guideRunning = false;
                }
                else
                {
                    WrongInput.Show();
                }
            }
        }
    }

    private static bool IsValidLanguage(string language)
    {
        return language.ToLower() == "e" || language.ToLower() == "english";
    }
}