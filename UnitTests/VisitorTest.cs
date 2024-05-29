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
            $"Duration: 40 min\n" +
            $"Language: English\n"
        ));
    }

    [TestMethod]
    public void ReservationAlreadyMadeTest()
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
            },
            {
                ""ID"": 2,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T13:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            }
        ]
        ";

        museum.Files[filePath1] = @"
        [
            {
                ""Id"": 1,
                ""QR"": ""78912"",
                ""TourId"": 1
            }
        ]
        ";
        TestableVisitor fakeVisitor = new TestableVisitor(museum);
        Visitor visitor = new Visitor(0, "78912");

        //Act
        bool result = fakeVisitor.Reservate(2, visitor);

        //Assert
        Assert.IsFalse(result);
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains("Maximum reservation limit reached."));
    }

    [TestMethod]
    public void ReservationTourNotAvailableTest()
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
        Visitor visitor = new Visitor(0, "89124");

        //Act
        fakeVisitor.Reservate(2, visitor);

        //Assert
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains("Tour is not available."));
    }

    [TestMethod]
    public void ReservationTourFullTest()
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
        Visitor visitor1 = new Visitor(0, "89124");
        Visitor visitor2 = new Visitor(0, "26546");
        Visitor visitor3 = new Visitor(0, "65743");
        Visitor visitor4 = new Visitor(0, "1244215");
        Visitor visitor5 = new Visitor(0, "54745");
        Visitor visitor6 = new Visitor(0, "789235");
        Visitor visitor7 = new Visitor(0, "865856");
        Visitor visitor8 = new Visitor(0, "74536");
        Visitor visitor9 = new Visitor(0, "32452");
        Visitor visitor10 = new Visitor(0, "232664");
        Visitor visitor11 = new Visitor(0, "86334");
        Visitor visitor12 = new Visitor(0, "23432");
        Visitor visitor13 = new Visitor(0, "54353");
        Visitor visitor14 = new Visitor(0, "25123");

        //Act
        fakeVisitor.Reservate(1, visitor1);
        fakeVisitor.Reservate(1, visitor2);
        fakeVisitor.Reservate(1, visitor3);
        fakeVisitor.Reservate(1, visitor4);
        fakeVisitor.Reservate(1, visitor5);
        fakeVisitor.Reservate(1, visitor6);
        fakeVisitor.Reservate(1, visitor7);
        fakeVisitor.Reservate(1, visitor8);
        fakeVisitor.Reservate(1, visitor9);
        fakeVisitor.Reservate(1, visitor10);
        fakeVisitor.Reservate(1, visitor11);
        fakeVisitor.Reservate(1, visitor12);
        fakeVisitor.Reservate(1, visitor13);
        fakeVisitor.Reservate(1, visitor14);

        //Assert
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains("Tour is full."));
    }

    [TestMethod]
    public void ViewReservationsMadeTest()
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
            {
                ""Id"": 1,
                ""TourId"": 1,
                ""QR"": ""647832""
            }
        ]
        ";
        
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
        TestableVisitor fakeVisitor = new TestableVisitor(museum);
        Visitor visitor = new Visitor(0, "647832");

        //Act
        bool result = fakeVisitor.ViewReservationsMade(visitor.QR);

        //Assert
        Assert.IsTrue(result);
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains(
            $"Date: 11-5-2024\n" +
            $"Time: 11:30\n" +
            $"Duration: 40 min\n" +
            $"Language: English\n"
        ));
    }
    

    [TestMethod]
    public void ViewReservationsMadeNoneTest()
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
        Visitor visitor = new Visitor(0, "879423");

        //Act
        bool result = fakeVisitor.ViewReservationsMade(visitor.QR);

        //Assert
        Assert.IsFalse(result);
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains("No reservation made."));
    }

    [TestMethod]
    public void CancelReservationTest()
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
            {
                ""Id"": 1,
                ""TourId"": 1,
                ""QR"": ""1567124""
            }
        ]
        ";

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
        
        museum.LinesToRead = new List<string> { "y" };

        TestableVisitor fakeVisitor = new TestableVisitor(museum);
        Visitor visitor = new Visitor(0, "1567124");

        //Act
        fakeVisitor.CancelReservation(visitor);
        
        //Assert
        bool result = fakeVisitor.ViewReservationsMade("1567124");
        Assert.IsFalse(result);
        Assert.IsTrue(museum.LinesWritten.Contains("Reservation cancelled successfully."));
    }

    [TestMethod]
    public void CancelReservationCancelledTest()
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
            {
                ""Id"": 1,
                ""TourId"": 1,
                ""QR"": ""1567124""
            }
        ]
        ";

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
        
        museum.LinesToRead = new List<string> { "n" };

        TestableVisitor fakeVisitor = new TestableVisitor(museum);
        Visitor visitor = new Visitor(0, "1567124");

        //Act
        fakeVisitor.CancelReservation(visitor);
        
        //Assert
        bool result = fakeVisitor.ViewReservationsMade("1567124");
        Assert.IsTrue(result);
        Assert.IsTrue(museum.LinesWritten.Contains("Reservation cancellation cancelled."));
    }

    [TestMethod]
    public void CancelReservationWrongInputTest()
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
            {
                ""Id"": 1,
                ""TourId"": 1,
                ""QR"": ""1567124""
            }
        ]
        ";

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
        
        museum.LinesToRead = new List<string> { "r" };

        TestableVisitor fakeVisitor = new TestableVisitor(museum);
        Visitor visitor = new Visitor(0, "1567124");

        //Act
        fakeVisitor.CancelReservation(visitor);
        
        //Assert
        bool result = fakeVisitor.ViewReservationsMade("1567124");
        Assert.IsTrue(result);
        Assert.IsTrue(museum.LinesWritten.Contains("Wrong input. Try again."));
    }
}