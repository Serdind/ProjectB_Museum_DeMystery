using NUnit.Framework.Internal;

namespace UnitTests;

[TestClass]
public class GuideTests
{
    [TestMethod]
    public void GuideTest()
    {
        // Arrange
        Guide guide = new Guide("TestUser", "682374");
        
        // Assert
        Assert.IsNotNull(guide);
    }

    [TestMethod]
    public void ViewToursTest()
    {
        
    }

    [TestMethod]
    public void StartTourTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        TestableGuide guide = new TestableGuide(museum);
        GuidedTour tour = new GuidedTour("TestName", DateTime.Now, "Dutch", "TestGuide");

        // Act
        guide.StartTour(tour);

        // Assert
        Assert.AreEqual("Tour has been started:Tour: TestName\nDate: " + DateTime.Now.ToShortDateString() + "\nTime: " + DateTime.Now.ToString("HH:mm") + "\nLanguage: Dutch\n", museum.GetWrittenLinesAsString());
    }

    [TestMethod]
    public void StartTourTourNotFoundTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        TestableGuide guide = new TestableGuide(museum);
        GuidedTour nonExistentTour = null;

        // Act
        guide.StartTour(nonExistentTour);

        // Assert
        Assert.AreEqual("Tour not found.", museum.GetWrittenLinesAsString().Trim());
    }
}