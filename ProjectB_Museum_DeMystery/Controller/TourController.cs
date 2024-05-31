public class TourController
{
    public bool ReservateTour(Visitor visitor)
    {
        bool toursAvailable = Tour.OverviewTours();
        if (!toursAvailable)
        {
            return false;
        };

        AdminOptions.BackOption();
        int tourID = TourId.WhichTourId();

        if (tourID == -1)
        {
            return false;
        }

        visitor.Reservate(tourID, visitor);
        return true;
    }
}