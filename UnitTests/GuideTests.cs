using NUnit.Framework.Internal;

namespace UnitTests;

[TestClass]
public class GuideTests
{
    [TestMethod]
    public void GuideTest()
    {
        //Arrange
        Guide guide = new Guide("TestUser", "682374");
        
        //Assert
        Assert.IsNotNull(guide);
    }

    [TestMethod]
    public void AddVisistorToTourTest()
    {
        //Arrange
        FakeMuseum museum = new FakeMuseum();
        Visitor visitor= new Visitor(0, "61823");
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        museum.Files[filePath] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            }
        ]
        ";
        TestableGuide guide = new TestableGuide(museum);

        //Act
        bool result = guide.AddVisistorToTour(1, visitor.QR);

        //Assert
        Assert.IsTrue(result);
        Assert.AreEqual("Succesfully added visitor to tour.", museum.GetWrittenLinesAsString());
    }

    [TestMethod]
    public void AddVisistorToTourNotFoundTest()
    {
        //Arrange
        FakeMuseum museum = new FakeMuseum();
        Visitor visitor= new Visitor(0, "61823");
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        museum.Files[filePath] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            }
        ]
        ";
        TestableGuide guide = new TestableGuide(museum);

        //Act
        bool result = guide.AddVisistorToTour(2, visitor.QR);

        //Assert
        Assert.IsFalse(result);
        Assert.AreEqual("Tour not found.", museum.GetWrittenLinesAsString());
    }

    [TestMethod]
    public void ViewToursTest()
    {
        
    }

    [TestMethod]
    public void StartTourTest()
    {
        //Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        museum.Files[filePath] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            }
        ]
        ";
        TestableGuide guide = new TestableGuide(museum);

        //Act
        guide.StartTour(1);

        //Assert
        Assert.AreEqual("Tour has been started:\nTour: Museum tour\nDate: 11-5-2024\nTime: 11:30\nLanguage: English\n", museum.GetWrittenLinesAsString());
    }

    [TestMethod]
    public void StartTourTourNotFoundTest()
    {
        //Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);
        museum.Files[filePath] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            }
        ]
        ";
        TestableGuide guide = new TestableGuide(museum);

        //Act
        guide.StartTour(2);

        //Assert
        Assert.AreEqual("Tour not found.", museum.GetWrittenLinesAsString().Trim());
    }
}