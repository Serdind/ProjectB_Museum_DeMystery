public class TestableTourController
{

    public readonly IMuseum Museum;

    public TestableTourController(IMuseum museum)
    {
        Museum = museum;
    }

    public void ReservateTour(Visitor visitor, TestableVisitor testVisitor)
    {
        int tourID = TourId.WhichTourId();

        testVisitor.Reservate(tourID, visitor);
    }
}