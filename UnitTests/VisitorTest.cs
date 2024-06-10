using System.Diagnostics;
using NUnit.Framework.Internal;
using Newtonsoft.Json;

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
    public void VistorLoginWithValidBarcodeTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath2 = Model<UniqueCodes>.GetFileNameUniqueCodes();

        museum.Files[filePath2] = @"
        [
            ""139278"",
            ""78643"",
            ""124678""
        ]
        ";

        string filePath3 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath3] = "[]";

        museum.LinesToRead = new List<string>
        {
            "78643",  // QR code input
            "b" // Back input
        };

        // Act
        ProgramController.Start();

        // Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        Assert.IsTrue(writtenLines.Contains("Scan the barcode that is located on the ticket you bought with the given device. Press the button and hold the scanner of the device closely to the barcode:"));
    }

    [TestMethod]
    public void VistorLoginWithInvalidBarcodeTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath2 = Model<UniqueCodes>.GetFileNameUniqueCodes();

        museum.Files[filePath2] = @"
        [
            ""139278"",
            ""78643"",
            ""124678""
        ]
        ";

        string filePath3 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath3] = "[]";

        museum.LinesToRead = new List<string>
        {
            "641223",  // QR code input
            "b" // Back input
        };

        // Act
        ProgramController.Start();

        // Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        Assert.IsTrue(writtenLines.Contains("Code is not valid."));
    }

    [TestMethod]
    public void ReservationSuccesTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            
        ]";

        Visitor visitor = new Visitor(0, "523523");

        //Act
        bool result = visitor.Reservate(1, visitor);

        //Assert
        string message = $"Reservation successful. You have reserved the following tour:\n" +
                    $"Date: {currentDate.ToShortDateString()}\n" +
                    $"Time: {currentDate.ToString("HH:mm")}\n" +
                    $"Duration: 40 minutes\n" +
                    $"Language: English\n" +
                    $"Name of guide: TestGuide\n";

        Assert.IsTrue(result);
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains(message));
    }

    [TestMethod]
    public void ReservationCapacityTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            
        ]";

        Visitor visitor = new Visitor(0, "523523");

        //Act
        bool result = visitor.Reservate(1, visitor);

        //Assert
        var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(museum.Files[filePath1]);
        var tour = tours.FirstOrDefault(t => t.ID == 1);
        Debug.WriteLine(tour.ReservedVisitors.Count());
        Assert.IsTrue(tour.MaxParticipants - tour.ReservedVisitors.Count() == 12);
    }

    [TestMethod]
    public void ReservationAlreadyMadeTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }},
            {{
                ""ID"": ""2"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": 1,
                ""QR"": ""523523"",
                ""TourId"": 1
            }
        ]";

        Visitor visitor = new Visitor(0, "523523");

        //Act
        bool result = visitor.Reservate(2, visitor);

        //Assert
        Assert.IsFalse(result);
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains("You already made a reservation for today."));
    }

    [TestMethod]
    public void ReservationTourNotAvailableTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }},
            {{
                ""ID"": ""2"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            
        ]";

        Visitor visitor = new Visitor(0, "523523");

        //Act
        bool result = visitor.Reservate(3, visitor);

        //Assert
        Assert.IsFalse(result);
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains("Tour is not available."));
    }

    [TestMethod]
    public void ReservationTourFullTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            
        ]";

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
        visitor1.Reservate(1, visitor1);
        visitor2.Reservate(1, visitor2);
        visitor3.Reservate(1, visitor3);
        visitor4.Reservate(1, visitor4);
        visitor5.Reservate(1, visitor5);
        visitor6.Reservate(1, visitor6);
        visitor7.Reservate(1, visitor7);
        visitor8.Reservate(1, visitor8);
        visitor9.Reservate(1, visitor9);
        visitor10.Reservate(1, visitor10);
        visitor11.Reservate(1, visitor11);
        visitor12.Reservate(1, visitor12);
        visitor13.Reservate(1, visitor13);
        visitor14.Reservate(1, visitor14);

        //Assert
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains("Tour is full."));
    }

    [TestMethod]
    public void ViewReservationsMadeTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": 1,
                ""QR"": ""647832"",
                ""TourId"": 1
            } 
        ]";
        Visitor visitor = new Visitor(0, "647832");

        //Act
        bool result = visitor.ViewReservationsMade(visitor.QR);

        //Assert
        string message = $"Date: {currentDate.ToShortDateString()}\n" +
                        $"Time: {currentDate.ToString("HH:mm")}\n" +
                        $"Duration: 40 minutes\n" +
                        $"Language: English\n" +
                        $"Name of guide: TestGuide\n";

        Assert.IsTrue(result);
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains(message));
    }

    [TestMethod]
    public void ViewReservationsMadeNoneTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            
        ]";

        Visitor visitor = new Visitor(0, "879423");

        //Act
        bool result = visitor.ViewReservationsMade(visitor.QR);

        //Assert
        Assert.IsFalse(result);
        Debug.WriteLine(museum.GetWrittenLinesAsString());
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains("No reservation made."));
    }

    [TestMethod]
    public void CancelReservationTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": 1,
                ""TourId"": 1,
                ""QR"": ""1567124""
            }
        ]";
        
        museum.LinesToRead = new List<string> { "y" };

        Visitor visitor = new Visitor(0, "1567124");

        //Act
        visitor.CancelReservation(visitor);
        
        //Assert
        bool result = visitor.ViewReservationsMade("1567124");
        Assert.IsFalse(result);
        Assert.IsTrue(museum.LinesWritten.Contains("Reservation cancelled successfully."));
    }

    [TestMethod]
    public void CancelReservationCancelledTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": 1,
                ""TourId"": 1,
                ""QR"": ""1567124""
            }
        ]";
        
        museum.LinesToRead = new List<string> { "n" };

        Visitor visitor = new Visitor(0, "1567124");

        //Act
        visitor.CancelReservation(visitor);
        
        //Assert
        bool result = visitor.ViewReservationsMade("1567124");
        Assert.IsTrue(result);
        Assert.IsTrue(museum.LinesWritten.Contains("Reservation cancellation cancelled."));
    }

    [TestMethod]
    public void OverviewToursWithRemainingSpotsTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Date"": ""{currentDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }}
        ]
        ";

        museum.Files[filePath1] = toursJson;

        museum.LinesToRead = new List<string>
        {
            "t",
            "l"
        };

        // Act
        bool result = Tour.OverviewTours(true);

        // Assert
        Assert.IsTrue(result);

        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);

        Assert.IsTrue(writtenLines.Contains("Remaining Spots"));
        Assert.IsTrue(writtenLines.Contains("13"));
    }
}