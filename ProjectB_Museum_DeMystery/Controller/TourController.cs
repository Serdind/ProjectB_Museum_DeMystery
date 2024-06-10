public class TourController
{
    
    public bool ReservateTour(Visitor visitor)
    {
        int tourID;

        do
        {
            AdminOptions.BackOption();
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

            if (tourID == 0)
            {
                continue;
            }

            if (visitor.Reservate(tourID, visitor))
            {
                return true;
            }
        } while (true);
    }
}