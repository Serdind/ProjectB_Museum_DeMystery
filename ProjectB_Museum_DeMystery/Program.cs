class Program
{
    public static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("Reservate(R)\nQuit(Q)");
            string option = Console.ReadLine();

            Console.WriteLine("Insert your full name:");
            string name = Console.ReadLine();
            Console.WriteLine("Insert your email:");
            string email = Console.ReadLine();
            Console.WriteLine("Insert your phonenumber:");
            string phonenumber = Console.ReadLine();


            Visitor visitor = new Visitor(name, email, phonenumber);
            
            Tours.OverviewTours();

            if (option.ToLower() == "r")
            {
                Console.WriteLine("Which tour? (ID)");
                int tourID = Convert.ToInt32(Console.ReadLine());
                bool tourFound = false;

                foreach (GuidedTour tour in Tours.guidedTour)
                {
                    if (tourID == tour.ID)
                    {
                        tour.PlaceReservation(tourID, visitor);
                        tourFound = true;
                        break;
                    }
                }

                if (!tourFound)
                {
                    Console.WriteLine("Tour not found.\n");
                }
            }
        }
    }
}