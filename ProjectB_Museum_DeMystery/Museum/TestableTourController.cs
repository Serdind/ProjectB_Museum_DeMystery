public class TestableTourController
{

    public readonly IMuseum Museum;

    public TestableTourController(IMuseum museum)
    {
        Museum = museum;
    }

    public void ReservateTour(Visitor visitor, TestableVisitor testVisitor)
    {
        TestableTour testableTour = new TestableTour(Museum);
        
        bool toursAvailable = testableTour.OverviewTours(false);
        if (!toursAvailable)
        {
            return;
        };

        int tourID = TourId.WhichTourId();

        testVisitor.Reservate(tourID, visitor);
    }
}