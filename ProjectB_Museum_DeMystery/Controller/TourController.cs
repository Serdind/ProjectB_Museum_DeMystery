public static class TourController
{
    public static void ReservateTour(Visitor visitor)
    {
        Tour.OverviewTours(false);
        int tourID = TourId.WhichTourId();

        visitor.Reservate(tourID, visitor);
    }
}