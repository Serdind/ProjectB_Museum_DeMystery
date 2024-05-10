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

        // Act
        guide.StartTour(1);

        // Assert
        Assert.AreEqual("Tour has been started:\nTour: Museum tour\nDate: 10-5-2024\nTime: 11:30\nLanguage: English\n", museum.GetWrittenLinesAsString());
    }

    [TestMethod]
    public void StartTourTourNotFoundTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        TestableGuide guide = new TestableGuide(museum);

        // Act
        guide.StartTour(999);

        // Assert
        Assert.AreEqual("Tour not found.", museum.GetWrittenLinesAsString().Trim());
    }
}