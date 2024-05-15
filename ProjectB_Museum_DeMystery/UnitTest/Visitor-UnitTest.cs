using NUnit.Framework.Internal;

namespace UnitTests;

[TestClass]
public class VisitorTests
{

    [TestMethod]
    public void VisitorTest()
    {
        //Arrange
        Visitor visitor = new Visitor(0, "78423");
        
        //Assert
        Assert.IsNotNull(visitor);
    }

    [TestMethod]
    public void ReservationSuccesTest()
    {
        //Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName1 = "visitorsTest.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);


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

        museum.Files[filePath1] = @"
        [
            
        ]
        ";
        TestableVisitor fakeVisitor = new TestableVisitor(museum);
        Visitor visitor = new Visitor(0, "523523");

        //Act
        bool result = fakeVisitor.Reservate(1, visitor);

        //Assert
        Assert.IsTrue(result);
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains(
            $"Reservation successful. You have reserved the following tour:\n" +
            $"Date: 11-5-2024\n" +
            $"Time: 11:30\n" +
            $"Duration: 20 min\n" +
            $"Language: English\n"
        ));
    }
}