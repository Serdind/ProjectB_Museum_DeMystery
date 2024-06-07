public class TourController
{
    private static IMuseum museum = Program.Museum;
    public bool ReservateTour(Visitor visitor)
    {
        
        AdminOptions.BackOption();
        int tourID;

        do
        {
            bool toursAvailable = Tour.OverviewTours(false);
            if (!toursAvailable)
            {
                return false;
            }

            tourID = TourId.WhichTourId();

            if (tourID == -1)
            {
                return false;
            }

            if (visitor.Reservate(tourID, visitor))
            {
                return true;
            }
        } while (true);
    }
}