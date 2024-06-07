using NUnit.Framework.Internal;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;

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

        string filePath2 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": ""1"",
                ""Name"": ""TestGuide"",
                ""QR"": ""214678""
            }
        ]
        ";

        string filePath3 = Model<UniqueCodes>.GetFileNameUniqueCodes();

        museum.Files[filePath3] = @"
        [
            ""139278"",
            ""78643"",
            ""124678""
        ]
        ";

        string filePath4 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath4] = "[]";

        var guides = JsonConvert.DeserializeObject<List<Guide>>(museum.Files[filePath2]);
        var guide = guides.FirstOrDefault(t => t.QR == "214678");

        //Act
        bool result = guide.AddVisitorToTour(1, "78643");

        //Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        Assert.IsTrue(result);
        Assert.IsTrue(writtenLines.Contains("Succesfully added visitor to tour."));
    }

    [TestMethod]
    public void AddVisistorToTourNotFoundTest()
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

        string filePath2 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": ""1"",
                ""Name"": ""TestGuide"",
                ""QR"": ""214678""
            }
        ]
        ";

        string filePath3 = Model<UniqueCodes>.GetFileNameUniqueCodes();

        museum.Files[filePath3] = @"
        [
            ""139278"",
            ""78643"",
            ""124678""
        ]
        ";

        string filePath4 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath4] = "[]";

        var guides = JsonConvert.DeserializeObject<List<Guide>>(museum.Files[filePath2]);
        var guide = guides.FirstOrDefault(t => t.QR == "214678");

        //Act
        bool result = guide.AddVisitorToTour(2, "78643");

        //Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        Assert.IsFalse(result);
        Assert.IsTrue(writtenLines.Contains("Tour not found."));
    }

    [TestMethod]
    public void RemoveVisitorFromTourTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = @"
        [
            {
                ""ID"": ""1"",
                ""Date"": """ + currentDateString + @""",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [{
                        ""Id"": ""1"",
                        ""TourId"": ""1"",
                        ""QR"": ""78643""
                    }],
                ""Language"": ""English"",
                ""Status"": true
            }
        ]";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": ""1"",
                ""Name"": ""TestGuide"",
                ""QR"": ""214678""
            }
        ]
        ";

        string filePath3 = Model<UniqueCodes>.GetFileNameUniqueCodes();

        museum.Files[filePath3] = @"
        [
            ""139278"",
            ""78643"",
            ""124678""
        ]
        ";

        string filePath4 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath4] = @"
        [
            {
                ""Id"": ""1"",
                ""TourId"": ""1"",
                ""QR"": ""78643""
            }
        ]";

        var guides = JsonConvert.DeserializeObject<List<Guide>>(museum.Files[filePath2]);
        var guide = guides.FirstOrDefault(t => t.QR == "214678");

        //Act
        bool result = guide.RemoveVisitorFromTour(1, "78643");

        //Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        Assert.IsTrue(result);
        Assert.IsTrue(writtenLines.Contains("Succesfully removed visitor from tour."));
    }

    [TestMethod]
    public void RemoveVisitorFromTourFailedTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = @"
        [
            {
                ""ID"": ""1"",
                ""Date"": """ + currentDateString + @""",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [{
                        ""Id"": ""1"",
                        ""TourId"": ""1"",
                        ""QR"": ""78643""
                    }],
                ""Language"": ""English"",
                ""Status"": true
            }
        ]";

        museum.Files[filePath1] = toursJson;

        string filePath2 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": ""1"",
                ""Name"": ""TestGuide"",
                ""QR"": ""214678""
            }
        ]
        ";

        string filePath3 = Model<UniqueCodes>.GetFileNameUniqueCodes();

        museum.Files[filePath3] = @"
        [
            ""139278"",
            ""78643"",
            ""124678""
        ]
        ";

        string filePath4 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath4] = @"
        [
            {
                ""Id"": ""1"",
                ""TourId"": ""1"",
                ""QR"": ""78643""
            }
        ]";

        var guides = JsonConvert.DeserializeObject<List<Guide>>(museum.Files[filePath2]);
        var guide = guides.FirstOrDefault(t => t.QR == "214678");

        //Act
        bool result = guide.RemoveVisitorFromTour(1, "321745");

        //Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        Assert.IsFalse(result);
        Assert.IsTrue(writtenLines.Contains("Visitor not found in the tour's list of reserved visitors."));
    }

    [TestMethod]
    public void ViewToursTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today;
        currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

        string pastTourDateString = currentDate.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss");
        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        DateTime currentDateTime = DateTime.ParseExact(currentDateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": ""1"",
                ""Date"": ""{pastTourDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }},
            {{
                ""ID"": ""2"",
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

        string filePath2 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": ""1"",
                ""Name"": ""TestGuide"",
                ""QR"": ""214678""
            }
        ]
        ";

        string filePath3 = Model<UniqueCodes>.GetFileNameUniqueCodes();

        museum.Files[filePath3] = @"
        [
            ""139278"",
            ""78643"",
            ""124678""
        ]
        ";

        string filePath4 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath4] = @"
        [
            {
                ""Id"": ""1"",
                ""TourId"": ""1"",
                ""QR"": ""78643""
            }
        ]";

        museum.LinesToRead = new List<string>
        {
            "b", // Back input
            "l" // Log out input
        };

        var guides = JsonConvert.DeserializeObject<List<Guide>>(museum.Files[filePath2]);
        var guide = guides.FirstOrDefault(t => t.QR == "214678");

        //Act
        bool result = guide.ViewTours(guide.Name, guide);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void StartTourTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today.AddHours(23).AddMinutes(59);

        string pastTourDateString = currentDate.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss");
        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        DateTime currentDateTime = DateTime.ParseExact(currentDateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        
        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": 1,
                ""Date"": ""{pastTourDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }},
            {{
                ""ID"": 2,
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

        string filePath2 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": 1,
                ""Name"": ""TestGuide"",
                ""QR"": ""214678""
            }
        ]
        ";

        // Act
        var guides = JsonConvert.DeserializeObject<List<Guide>>(museum.Files[filePath2]);
        var guide = guides.FirstOrDefault(t => t.QR == "214678");

        // Act
        guide.StartTour(2);

        // Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        Assert.IsTrue(writtenLines.Contains("Tour has been started"));
    }

    [TestMethod]
    public void StartTourInactiveTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today.AddHours(23).AddMinutes(59);

        string pastTourDateString = currentDate.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss");
        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        DateTime currentDateTime = DateTime.ParseExact(currentDateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        
        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": 1,
                ""Date"": ""{pastTourDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }},
            {{
                ""ID"": 2,
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

        string filePath2 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": 1,
                ""Name"": ""TestGuide"",
                ""QR"": ""214678""
            }
        ]
        ";

        // Act
        var guides = JsonConvert.DeserializeObject<List<Guide>>(museum.Files[filePath2]);
        var guide = guides.FirstOrDefault(t => t.QR == "214678");

        // Act
        guide.StartTour(2);

        // Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(museum.Files[filePath1]);
        var tour = tours.FirstOrDefault(t => t.ID == 2);
	    Assert.IsTrue(tour.Status == false);
    }

    [TestMethod]
    public void StartTourTourNotFoundTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        DateTime currentDate = DateTime.Today.AddHours(23).AddMinutes(59);

        string pastTourDateString = currentDate.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss");
        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");

        DateTime currentDateTime = DateTime.ParseExact(currentDateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        
        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            {{
                ""ID"": 1,
                ""Date"": ""{pastTourDateString}"",
                ""NameGuide"": ""TestGuide"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }},
            {{
                ""ID"": 2,
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

        string filePath2 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": 1,
                ""Name"": ""TestGuide"",
                ""QR"": ""214678""
            }
        ]
        ";

        // Act
        var guides = JsonConvert.DeserializeObject<List<Guide>>(museum.Files[filePath2]);
        var guide = guides.FirstOrDefault(t => t.QR == "214678");

        // Act
        guide.StartTour(4);

        // Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        Assert.IsTrue(writtenLines.Contains("Tour not found."));
    }
}