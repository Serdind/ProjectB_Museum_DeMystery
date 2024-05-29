public class TestableTourController
{

    public readonly IMuseum Museum;

    public TestableTourController(IMuseum museum)
    {
        Museum = museum;
    }

    public void ReservateTour(Visitor visitor, TestableVisitor testVisitor)
    {
        TestableTourId testableTourId = new TestableTourId(Museum);
        int tourID = testableTourId.WhichTourId();

        testVisitor.Reservate(tourID, visitor);
    }
}