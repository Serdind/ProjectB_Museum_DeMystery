public class TestableProgramController
{

    public readonly IMuseum Museum;

    public TestableProgramController(IMuseum museum)
    {
        Museum = museum;
    }
    public void Start()
    {
        TestableVisitor visitorMethods = new TestableVisitor(Museum);
        TestablePerson personMethods = new TestablePerson(Museum);

        Visitor visitor = new Visitor(0, null);
        bool showIntro = true;

        string language;
        do
        {
            if (showIntro)
            {
                Museum.WriteLine("Here are some tips to help you with the application.");
                Museum.WriteLine("Press any key to continue...\n");
                Museum.ReadKey();
                Museum.WriteLine("When you have to choose between options, you can write the whole option name or you can write the given letter.");
                Museum.WriteLine("Press any key to continue...\n");
                Museum.ReadKey();
                Museum.WriteLine("It doesn't matter if it's lowercase (example: abc) or uppercase (example: ABC).");
                Museum.WriteLine("Press any key to continue...\n");
                Museum.ReadKey();
                Museum.WriteLine("For example, the options are: Make reservation(R) and Cancel reservation(C)");
                Museum.WriteLine("Press any key to continue...\n");
                Museum.ReadKey();
                Museum.WriteLine("You can choose to insert the whole option name, for example: Make reservation, or you can choose R.");
                Museum.WriteLine("Press any key to continue...\n");
                Museum.ReadKey();
                Museum.WriteLine("It's recommended to insert the given letter.");
                Museum.WriteLine("Press any key to continue...\n");
                Museum.ReadKey();
                Museum.WriteLine("Press enter when the desired input is given.");
                Museum.WriteLine("Press any key to continue...\n");
                Museum.ReadKey();
                showIntro = false;
            }
            Museum.WriteLine("Welcome to Het Depot!");
            Museum.WriteLine("Press any key to continue...\n");
            Museum.ReadKey();
            Museum.WriteLine("Select language");
            Museum.WriteLine("English(E)");
            language = Museum.ReadLine();

            if (!IsValidLanguage(language))
            {
                Museum.WriteLine("Invalid language selection. Please choose again.");
            }

        } while (!IsValidLanguage(language));

        language = language.ToLower();

        string choice;
        bool showLoginPrompt = true;

        do
        {
            if (showLoginPrompt)
            {
                Museum.WriteLine("Login(L)");
                choice = Museum.ReadLine();
                showLoginPrompt = false;
            }
            else
            {
                Museum.WriteLine("Login(L)");
                choice = Museum.ReadLine();
            }

            if (choice.ToLower() != "l" && choice.ToLower() != "login")
            {
                Museum.WriteLine("Invalid choice. Please try again.");
            }

        } while (choice.ToLower() != "l" && choice.ToLower() != "login");

        Museum.WriteLine("Scan your QR code:");
        string qr = Museum.ReadLine();
        bool accountCreated = personMethods.AccCreated(qr);
        string loginStatus;

        if (accountCreated)
        {
            loginStatus = "Visitor";
            visitor.QR = qr;
        }
        else
        {
            loginStatus = personMethods.Login(qr);
        }

        if (loginStatus == "Visitor")
        {
            bool visitorRunning = true;
            while (visitorRunning)
            {
                Museum.WriteLine("Make reservation(R)");

                if (visitor.ReservationMade(qr))
                {
                    Museum.WriteLine("My reservations(M)");
                    Museum.WriteLine("Cancel reservation(C)");
                }
                Museum.WriteLine("Log out(L)");

                string option = Museum.ReadLine();

                if (option.ToLower() == "r" || option.ToLower() == "make reservation")
                {
                    TestableTourController tourController = new TestableTourController(Museum);
                    tourController.ReservateTour(visitor, visitorMethods);
                }
                else if (option.ToLower() == "m" || option.ToLower() == "my reservations")
                {
                    visitorMethods.ViewReservationsMade(qr);
                }
                else if (option.ToLower() == "c" || option.ToLower() == "cancel reservation")
                {
                    visitorMethods.CancelReservation(visitor);
                }
                else if (option.ToLower() == "l" || option.ToLower() == "log out")
                {
                    visitorRunning = false;
                }
                else
                {
                    Museum.WriteLine("Wrong input. Try again.");
                }
            }
        }
        else if (loginStatus == "Admin")
        {
            TestablePersonController personController = new TestablePersonController(Museum);
            personController.AdminMenu(language);
        }
        else if (loginStatus == "Guide")
        {
            bool guideRunning = true;
            TestableGuide testableGuide = new TestableGuide(Museum);

            while (guideRunning)
            {
                
                Museum.WriteLine("My tours(M)\nLog out(L)");
                string option = Museum.ReadLine();

                if (option.ToLower() == "m" || option.ToLower() == "my tours")
                {
                    testableGuide.ViewTours("TestGuide");
                }
                else if (option.ToLower() == "l" || option.ToLower() == "log out")
                {
                    guideRunning = false;
                }
                else
                {
                    Museum.WriteLine("Wrong input. Try again.");
                }
            }
        }
    }

    private bool IsValidLanguage(string language)
    {
        return language.ToLower() == "e" || language.ToLower() == "english";
    }
}