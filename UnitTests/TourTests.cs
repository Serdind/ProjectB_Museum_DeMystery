using NUnit.Framework.Internal;
using Newtonsoft.Json;
using Spectre.Console;
using System.Diagnostics;
using System.Text;

namespace UnitTests;

[TestClass]
public class TourTests
{    
    [TestMethod]
    public void UpdateToursTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<GuidedTour>.GetFileNameTours();
        museum.Files[filePath1] = "[]";

        // Act
        Tour.UpdateTours();

        // Assert
        var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(museum.ReadAllText(filePath1));
        Assert.IsTrue(museum.Files.ContainsKey(filePath1));
        Assert.IsNotNull(tours);
    }

    [TestMethod]
    public void AddTourTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        List<GuidedTour> tours = new List<GuidedTour>();

        // Act
        GuidedTour newTour = new GuidedTour(museum.Now.AddDays(1), "English", "TestGuide");
        Tour.AddTour(newTour, tours);

        // Assert
        Assert.AreEqual(1, tours.Count);
    }

    [TestMethod]
    public void SaveToursToFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        museum.Files[filePath1] = "[]";

        GuidedTour existingTour1 = new GuidedTour(museum.Now, "English", "TestGuide");
        GuidedTour existingTour2 = new GuidedTour(museum.Now, "English", "TestGuide");

        var existingTours = new List<GuidedTour> {existingTour1, existingTour2};

        string existingJson = JsonConvert.SerializeObject(existingTours);
        museum.Files[filePath1] = existingJson;

        GuidedTour newTour = new GuidedTour(museum.Now.AddDays(1), "English", "TestGuide");
        var newTours = new List<GuidedTour> {newTour};

        // Act
        Tour.SaveToursToFile(filePath1, newTours);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath1));
        var savedToursJson = museum.Files[filePath1];
        var savedTours = JsonConvert.DeserializeObject<List<GuidedTour>>(savedToursJson);
        Assert.AreEqual(existingTours.Count + newTours.Count, savedTours.Count);
    }
    

    [TestMethod]
    public void OverviewToursTodayTest()
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

        StringBuilder outputBuilder = new StringBuilder();
        var originalConsoleOut = Console.Out;
        Console.SetOut(new StringWriter(outputBuilder));

        // Act
        bool result = Tour.OverviewTours(true);
        Console.SetOut(originalConsoleOut);

        // Assert
        Assert.IsTrue(result);

        string tableOutput = outputBuilder.ToString();

        Debug.WriteLine("Table Output:");
        Debug.WriteLine(tableOutput);

        Assert.IsTrue(tableOutput.Contains($"{currentDate.ToString("d-M-yyyy")}"));
    }

    [TestMethod]
    public void OverviewToursNoToursTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        string toursJson = $@"
        [
            
        ]
        ";

        museum.Files[filePath1] = toursJson;

        //Act
        Tour.OverviewTours(false);

        // Assert
        Assert.IsTrue(museum.GetWrittenLinesAsString().Contains("No tours available for today."));
    }

    [TestMethod]
    public void LoadToursFromFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<GuidedTour>.GetFileNameTours();

        GuidedTour tour1 = new GuidedTour(new DateTime(2024, 5, 11, 11, 30, 0), "English", "TestGuide");
        GuidedTour tour2 = new GuidedTour(new DateTime(2024, 5, 24, 11, 30, 0), "English", "TestGuide");

        var json = JsonConvert.SerializeObject(new List<GuidedTour> { tour1, tour2 });

        museum.Files[filePath1] = json;

        // Act
        var loadedTours = Tour.LoadToursFromFile();

        // Assert
        Assert.IsNotNull(loadedTours);
        Assert.AreEqual(2, loadedTours.Count);
        Assert.IsTrue(loadedTours.Any(t => t.Date.Date == new DateTime(2024, 5, 11).Date));
        Assert.IsTrue(loadedTours.Any(t => t.Date.Date == new DateTime(2024, 5, 24).Date));
    }

    [TestMethod]
    public void AddAdminToJSONTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        var adminsList = new List<DepartmentHead>();
        DepartmentHead departmentHead = new DepartmentHead("TestAdmin", "2983432");

        Tour.AddAdmin(departmentHead, adminsList);

        string filePath1 = Model<DepartmentHead>.GetFileNameAdmins();

        // Act
        Tour.AddAdminToJSON(adminsList);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath1));

        var json = museum.ReadAllText(filePath1);
        var admins = JsonConvert.DeserializeObject<List<DepartmentHead>>(json);

        Assert.IsNotNull(admins);
        Assert.IsTrue(admins.Any(t => t.Name == "TestAdmin"));
        Assert.IsTrue(admins.Any(t => t.QR == "2983432"));
    }

    [TestMethod]
    public void AddAdminTest()
    {
        // Arrange
        var adminsList = new List<DepartmentHead>();
        DepartmentHead departmentHead = new DepartmentHead("TestAdmin", "2983432");

        // Act
        Tour.AddAdmin(departmentHead, adminsList);

        // Assert
        Assert.IsTrue(adminsList.Contains(departmentHead));
    }

    [TestMethod]
    public void SaveAdminToFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;
        
        var adminsList = new List<DepartmentHead>();
        DepartmentHead departmentHead1 = new DepartmentHead("TestAdmin1", "2983432");
        DepartmentHead departmentHead2 = new DepartmentHead("TestAdmin2", "468712");
        
        Tour.AddAdmin(departmentHead1, adminsList);
        Tour.AddAdmin(departmentHead2, adminsList);

        string filePath1 = Model<DepartmentHead>.GetFileNameAdmins();

        // Act
        Tour.SaveAdminToFile(filePath1, adminsList);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath1));

        var json = museum.ReadAllText(filePath1);
        var savedAdmins = JsonConvert.DeserializeObject<List<DepartmentHead>>(json);

        Assert.IsNotNull(savedAdmins);
        Assert.AreEqual(2, savedAdmins.Count);

        Assert.IsTrue(savedAdmins.Any(a => a.Name == "TestAdmin1" && a.QR == "2983432"));
        Assert.IsTrue(savedAdmins.Any(a => a.Name == "TestAdmin2" && a.QR == "468712"));
    }

    [TestMethod]
    public void LoadAdminsFromFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<DepartmentHead>.GetFileNameAdmins();

        var admin1 = new DepartmentHead("TestAdmin1", "794832");
        var admin2 = new DepartmentHead("TestAdmin2", "1234567");
        var admins = new List<DepartmentHead> { admin1, admin2 };

        string json = JsonConvert.SerializeObject(admins);
        museum.Files[filePath1] = json;

        // Act
        var loadedAdmins = Tour.LoadAdminsFromFile();

        // Assert
        Assert.IsNotNull(loadedAdmins);
        Assert.AreEqual(2, loadedAdmins.Count);

        Assert.IsTrue(loadedAdmins.Any(a => a.Name == "TestAdmin1" && a.QR == "794832"));
        Assert.IsTrue(loadedAdmins.Any(a => a.Name == "TestAdmin2" && a.QR == "1234567"));
    }

    [TestMethod]
    public void AddGuideToJSONTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        var guidesList = new List<Guide>();
        Guide guide = new Guide("TestAdmin", "2983432");

        Tour.AddGuide(guide, guidesList);

        string filePath1 = Model<Guide>.GetFileNameGuides();

        // Act
        Tour.AddGuideToJSON(guidesList);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath1));

        var json = museum.ReadAllText(filePath1);
        var guides = JsonConvert.DeserializeObject<List<Guide>>(json);

        Assert.IsNotNull(guides);
        Assert.IsTrue(guides.Any(t => t.Name == "TestAdmin"));
        Assert.IsTrue(guides.Any(t => t.QR == "2983432"));
    }

    [TestMethod]
    public void AddGuideTest()
    {
        // Arrange
        var guidesList = new List<Guide>();
        Guide guide = new Guide("TestGuide", "21764821");

        // Act
        Tour.AddGuide(guide, guidesList);

        // Assert
        Assert.IsTrue(guidesList.Contains(guide));
    }

    [TestMethod]
    public void SaveGuideToFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        var guidesList = new List<Guide>();
        
        var guide1 = new Guide("TestGuide1", "794832");
        var guide2 = new Guide("TestGuide2", "1234567");
        
        Tour.AddGuide(guide1, guidesList);
        Tour.AddGuide(guide2, guidesList);

        string filePath1 = Model<Guide>.GetFileNameGuides();

        // Act
        Tour.SaveGuideToFile(filePath1, guidesList);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath1));

        var json = museum.ReadAllText(filePath1);
        var savedGuides = JsonConvert.DeserializeObject<List<Guide>>(json);

        Assert.IsNotNull(savedGuides);
        Assert.AreEqual(2, savedGuides.Count);

        Assert.IsTrue(savedGuides.Any(a => a.Name == "TestGuide1" && a.QR == "794832"));
        Assert.IsTrue(savedGuides.Any(a => a.Name == "TestGuide2" && a.QR == "1234567"));
    }

    [TestMethod]
    public void LoadGuidesFromFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<Guide>.GetFileNameGuides();

        var guide1 = new Guide("TestGuide1", "794832");
        var guide2 = new Guide("TestGuide2", "1234567");
        var guides = new List<Guide> { guide1, guide2 };

        string json = JsonConvert.SerializeObject(guides);
        museum.Files[filePath1] = json;

        // Act
        var loadedGuides = Tour.LoadGuidesFromFile();

        // Assert
        Assert.IsNotNull(loadedGuides);
        Assert.AreEqual(2, loadedGuides.Count);

        Assert.IsTrue(loadedGuides.Any(a => a.Name == "TestGuide1" && a.QR == "794832"));
        Assert.IsTrue(loadedGuides.Any(a => a.Name == "TestGuide2" && a.QR == "1234567"));
    }

    [TestMethod]
    public void AddVisitorToJSONTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<Visitor>.GetFileNameVisitors();

        int tourId = 1;
        string qr = "798217";

        // Act
        Tour.AddVisitorToJSON(tourId, qr);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath1));

        string jsonContent = museum.ReadAllText(filePath1);

        List<Visitor> visitors = JsonConvert.DeserializeObject<List<Visitor>>(jsonContent);

        Assert.IsNotNull(visitors);
        Assert.AreEqual(1, visitors.Count);

        Visitor addedVisitor = visitors[0];
        Assert.AreEqual(tourId, addedVisitor.TourId);
        Assert.AreEqual(qr, addedVisitor.QR);
    }

    [TestMethod]
    public void LoadVisitorsFromFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<Visitor>.GetFileNameVisitors();

        var visitor1 = new Visitor(1, "794832");
        var visitor2 = new Visitor(1, "1234567");
        var visitors = new List<Visitor> { visitor1, visitor2 };

        string json = JsonConvert.SerializeObject(visitors);
        museum.Files[filePath1] = json;

        // Act
        var loadedVisitors = Tour.LoadVisitorsFromFile();

        // Assert
        Assert.IsNotNull(loadedVisitors);
        Assert.AreEqual(2, loadedVisitors.Count);

        Assert.IsTrue(loadedVisitors.Any(a => a.QR == "794832"));
        Assert.IsTrue(loadedVisitors.Any(a => a.QR == "1234567"));
    }

    [TestMethod]
    public void AddVisitorTest()
    {
        // Arrange
        var visitorsList = new List<Visitor>();
        Visitor visitor = new Visitor(1, "21764821");

        // Act
        Tour.AddVisitor(visitor, visitorsList);

        // Assert
        Assert.IsTrue(visitorsList.Contains(visitor));
    }

    [TestMethod]
    public void SaveVisitorToFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        var visitorsList = new List<Visitor>();
        var visitor1 = new Visitor(1, "794832");
        var visitor2 = new Visitor(1, "1234567");
        
        Tour.AddVisitor(visitor1, visitorsList);
        Tour.AddVisitor(visitor2, visitorsList);

        string filePath1 = Model<Visitor>.GetFileNameVisitors();

        // Act
        Tour.SaveVisitorToFile(visitorsList);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath1));

        var json = museum.ReadAllText(filePath1);
        var savedVisitors = JsonConvert.DeserializeObject<List<Visitor>>(json);

        Assert.IsNotNull(savedVisitors);
        Assert.AreEqual(2, savedVisitors.Count);

        Assert.IsTrue(savedVisitors.Any(a => a.QR == "794832"));
        Assert.IsTrue(savedVisitors.Any(a => a.QR == "1234567"));
    }

    [TestMethod]
    public void OverviewVisitorsTour_FileExistsWithVisitorsForTour_VisitorsDisplayed()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<Visitor>.GetFileNameVisitors();

        List<Visitor> visitors = new List<Visitor>
        {
            new Visitor(1, "QR1"),
            new Visitor(2, "QR2"),
            new Visitor(1, "QR3")
        };
        string json = JsonConvert.SerializeObject(visitors);
        museum.Files[filePath1] = json;

        // Act
        bool result = Tour.OverviewVisitorsTour(1);

        // Assert
        string writtenLines = museum.GetWrittenLinesAsString();
        Debug.WriteLine(writtenLines);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CreateEmptyJsonFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<Visitor>.GetFileNameVisitors();

        // Act
        Tour.CreateEmptyJsonFile(filePath1);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath1));
        Assert.AreEqual("[]", museum.ReadAllText(filePath1));
    }
}