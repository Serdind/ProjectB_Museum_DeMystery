public class TourController
{
    private static IMuseum museum = Program.Museum;
    public bool ReservateTour(Visitor visitor)
    {
        bool toursAvailable = Tour.OverviewTours(false);
        if (!toursAvailable)
        {
            return false;
        }

        AdminOptions.BackOption();
        int tourID;

        do
        {
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