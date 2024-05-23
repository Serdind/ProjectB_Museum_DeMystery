public class TourController
{
    public void ReservateTour(Visitor visitor)
    {
        bool toursAvailable = Tour.OverviewTours(false);
        if (!toursAvailable)
        {
            return;
        };

        int tourID = TourId.WhichTourId();

        visitor.Reservate(tourID, visitor);
    }
}