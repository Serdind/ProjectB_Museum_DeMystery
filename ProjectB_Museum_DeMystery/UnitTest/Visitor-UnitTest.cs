using NUnit.Framework;

namespace VisitorUT.Tests
{
    [TestFixture]
    public class VisitorTests
    {
        private Visitor visitor;

        [SetUp]
        public void Setup()
        {
            string sampleQR = "11";
            visitor = new Visitor(sampleQR);
        }

        [Test]
        public void TestReservation_Succes()
        {
            string TourID = "sampleTourID";
            bool result = visitor.Reservate(TourID, visitor);
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestViewReservationsMade_None()
        {
            bool result = visitor.ViewReservationsMade(101);
            Assert.That(result, Is.False);
        }

        [Test]
        public void TestViewReservationsMade()
        {
            bool result = visitor.ViewReservationsMade(102);
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestCancelReservation_Succes()
        {
            visitor.CancelReservation(visitor);
            bool result = visitor.ViewReservationsMade(103);
            Assert.That(result, Is.False);
        }
    }
}
