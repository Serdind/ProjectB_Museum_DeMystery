using NUnit.Framework.Internal;
using Newtonsoft.Json;
using Spectre.Console;


namespace UnitTests;

[TestClass]
public class TourTests
{    
    [TestMethod]
    public void UpdateToursTest()
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
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
        
        TestableTour tour = new TestableTour(museum);

        var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(museum.ReadAllText(filePath));

        // Act
        tour.UpdateTours();

        // Assert
        Assert.IsTrue(museum.Files.ContainsKey(filePath));
        Assert.IsNotNull(tours);
        Assert.AreEqual(18, tours.Count);
    }

    [TestMethod]
    public void ToursDayTest()
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
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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
                ""ID"": 1,
                ""Name"": ""Museum tour"",
                ""Date"": ""2024-05-11T11:30:00"",
                ""Language"": ""English"",
                ""NameGuide"": ""Casper"",
                ""ReservedVisitors"": [],
                ""Status"": true
            },
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

        museum.Now = new DateTime(2024, 5, 24);

        var startDate = new DateTime(2024, 5, 24);
        var endDate = new DateTime(2024, 5, 24);

        var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(museum.ReadAllText(filePath));
        
        TestableTour tour = new TestableTour(museum);

        // Act
        tour.ToursDay(startDate, endDate);

        // Assert
        Assert.AreEqual(9 * ((endDate - startDate).Days + 1), tours.Count);
        Assert.IsTrue(museum.FileExists(filePath));
    }

    [TestMethod]
    public void SaveToursToFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        GuidedTour existingTour1 = new GuidedTour("ExistingTour1", museum.Now, "English", "TestGuide");
        GuidedTour existingTour2 = new GuidedTour("ExistingTour2", museum.Now, "English", "TestGuide");

        var existingTours = new List<GuidedTour> {existingTour1, existingTour2};

        string existingJson = JsonConvert.SerializeObject(existingTours);
        museum.Files[filePath] = existingJson;

        GuidedTour newTour = new GuidedTour("NewTour", museum.Now.AddDays(1), "English", "TestGuide");
        var newTours = new List<GuidedTour> {newTour};

        TestableTour tour = new TestableTour(museum);

        // Act
        tour.SaveToursToFile(filePath, newTours);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));
        var savedToursJson = museum.Files[filePath];
        var savedTours = JsonConvert.DeserializeObject<List<GuidedTour>>(savedToursJson);
        Assert.AreEqual(existingTours.Count + newTours.Count, savedTours.Count);
    }

    [TestMethod]
    public void AddTourTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);

        // Act
        tour.AddTour(new GuidedTour("NewTour", museum.Now.AddDays(1), "English", "TestGuide"));

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));
        var savedToursJson = museum.Files[filePath];
        var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(savedToursJson);
        Assert.AreEqual(1, tours.Count);
    }

    [TestMethod]
    public void OverviewToursTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);

        string json = @"[
            {
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""2024-05-25T10:00:00"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            },
            {
                ""ID"": ""2"",
                ""Name"": ""Tour 2"",
                ""Date"": ""2024-05-25T14:00:00"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
            }
        ]";
        museum.Files[filePath] = json;

        museum.LinesToRead = new List<string> { "25-5-2024" };

        //Act
        bool result = tour.OverviewTours(true);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void RemoveToursFromDateTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        GuidedTour tour1 = new GuidedTour("NewTour1", new DateTime(2024, 5, 11, 11, 30, 0), "English", "TestGuide");
        GuidedTour tour2 = new GuidedTour("NewTour2", new DateTime(2024, 5, 24, 11, 30, 0), "English", "TestGuide");

        var json = JsonConvert.SerializeObject(new List<GuidedTour> { tour1, tour2 });

        museum.Files[filePath] = json;

        TestableTour tour = new TestableTour(museum);

        // Act
        tour.RemoveToursFromDate(new DateTime(2024, 5, 11));

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));

        var savedToursJson = museum.Files[filePath];
        var savedTours = JsonConvert.DeserializeObject<List<GuidedTour>>(savedToursJson);

        Assert.IsFalse(savedTours.Any(t => t.Date.Date == new DateTime(2024, 5, 11).Date));
        Assert.IsTrue(savedTours.Any(t => t.Date.Date == new DateTime(2024, 5, 24).Date));
    }

    [TestMethod]
    public void LoadToursFromFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "toursTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        GuidedTour tour1 = new GuidedTour("NewTour1", new DateTime(2024, 5, 11, 11, 30, 0), "English", "TestGuide");
        GuidedTour tour2 = new GuidedTour("NewTour2", new DateTime(2024, 5, 24, 11, 30, 0), "English", "TestGuide");

        var json = JsonConvert.SerializeObject(new List<GuidedTour> { tour1, tour2 });

        museum.Files[filePath] = json;

        TestableTour tour = new TestableTour(museum);

        // Act
        var loadedTours = tour.LoadToursFromFile();

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
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "adminsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        // Act
        TestableTour tour = new TestableTour(museum);
        tour.AddAdminToJSON();

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));

        var json = museum.ReadAllText(filePath);
        var admins = JsonConvert.DeserializeObject<List<DepartmentHead>>(json);

        Assert.IsNotNull(admins);
        Assert.IsTrue(admins.Any(t => t.Name == "Frans"));
        Assert.IsTrue(admins.Any(t => t.QR == "6457823"));
    }

    [TestMethod]
    public void AddAdminTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        TestableTour tour = new TestableTour(museum);

        DepartmentHead departmentHead = new DepartmentHead("TestAdmin", "2983432");

        // Act
        tour.AddAdmin(departmentHead);

        // Assert
        Assert.IsTrue(tour.admins.Contains(departmentHead));
    }
    
    [TestMethod]
    public void SaveAdminToFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "adminsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);
        
        var admin1 = new DepartmentHead("TestAdmin1", "794832");
        var admin2 = new DepartmentHead("TestAdmin2", "1234567");
        
        tour.AddAdmin(admin1);
        tour.AddAdmin(admin2);

        // Act
        tour.SaveAdminToFile(filePath);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));

        var json = museum.ReadAllText(filePath);
        var savedAdmins = JsonConvert.DeserializeObject<List<DepartmentHead>>(json);

        Assert.IsNotNull(savedAdmins);
        Assert.AreEqual(2, savedAdmins.Count);

        Assert.IsTrue(savedAdmins.Any(a => a.Name == "TestAdmin1" && a.QR == "794832"));
        Assert.IsTrue(savedAdmins.Any(a => a.Name == "TestAdmin2" && a.QR == "1234567"));
    }

    [TestMethod]
    public void LoadAdminsFromFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "adminsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);

        var admin1 = new DepartmentHead("TestAdmin1", "794832");
        var admin2 = new DepartmentHead("TestAdmin2", "1234567");
        var admins = new List<DepartmentHead> { admin1, admin2 };

        string json = JsonConvert.SerializeObject(admins);
        museum.Files[filePath] = json;

        // Act
        var loadedAdmins = tour.LoadAdminsFromFile();

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
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "guidesTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        // Act
        TestableTour tour = new TestableTour(museum);
        tour.AddGuideToJSON();

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));

        var json = museum.ReadAllText(filePath);
        var guides = JsonConvert.DeserializeObject<List<DepartmentHead>>(json);

        Assert.IsNotNull(guides);
        Assert.IsTrue(guides.Any(t => t.Name == "Casper"));
        Assert.IsTrue(guides.Any(t => t.QR == "4892579"));
    }

    [TestMethod]
    public void AddGuideTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        TestableTour tour = new TestableTour(museum);

        Guide guide = new Guide("TestGuide", "21764821");

        // Act
        tour.AddGuide(guide);

        // Assert
        Assert.IsTrue(tour.guides.Contains(guide));
    }

    [TestMethod]
    public void SaveGuideToFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "guidesTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);
        
        var guide1 = new Guide("TestGuide1", "794832");
        var guide2 = new Guide("TestGuide2", "1234567");
        
        tour.AddGuide(guide1);
        tour.AddGuide(guide2);

        // Act
        tour.SaveGuideToFile(filePath);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));

        var json = museum.ReadAllText(filePath);
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
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "guidesTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);

        var guide1 = new Guide("TestGuide1", "794832");
        var guide2 = new Guide("TestGuide2", "1234567");
        var guides = new List<Guide> { guide1, guide2 };

        string json = JsonConvert.SerializeObject(guides);
        museum.Files[filePath] = json;

        // Act
        var loadedGuides = tour.LoadGuidesFromFile();

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
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);

        int tourId = 1;
        string qr = "798217";

        // Act
        tour.AddVisitorToJSON(tourId, qr);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));

        string jsonContent = museum.ReadAllText(filePath);

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
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);

        var visitor1 = new Visitor(1, "794832");
        var visitor2 = new Visitor(1, "1234567");
        var visitors = new List<Visitor> { visitor1, visitor2 };

        string json = JsonConvert.SerializeObject(visitors);
        museum.Files[filePath] = json;

        // Act
        var loadedVisitors = tour.LoadVisitorsFromFile();

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
        FakeMuseum museum = new FakeMuseum();
        TestableTour tour = new TestableTour(museum);

        Visitor visitor = new Visitor(1, "21764821");

        // Act
        tour.AddVisitor(visitor);

        // Assert
        Assert.IsTrue(tour.visitors.Contains(visitor));
    }

    [TestMethod]
    public void SaveVisitorToFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);
        
        var visitor1 = new Visitor(1, "794832");
        var visitor2 = new Visitor(1, "1234567");
        var visitors = new List<Visitor>() { visitor1, visitor2 };
        
        tour.AddVisitor(visitor1);
        tour.AddVisitor(visitor2);

        // Act
        tour.SaveVisitorToFile(visitors);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));

        var json = museum.ReadAllText(filePath);
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
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        List<Visitor> visitors = new List<Visitor>
        {
            new Visitor(1, "QR1"),
            new Visitor(2, "QR2"),
            new Visitor(1, "QR3")
        };
        string json = JsonConvert.SerializeObject(visitors);
        museum.Files[filePath] = json;

        TestableTour tour = new TestableTour(museum);

        // Act
        tour.OverviewVisitorsTour(1);

        // Assert
        string expectedOutput = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("ID")
            .AddColumn("Qr")
            .AddRow("1", "QR1")
            .AddRow("3", "QR3")
            .ToString();

        // Assert
        Assert.AreEqual(expectedOutput.Trim(), museum.GetWrittenLinesAsString().Trim());
    }

    [TestMethod]
    public void CreateEmptyJsonFileTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "visitorsTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour tour = new TestableTour(museum);

        // Act
        tour.CreateEmptyJsonFile(filePath);

        // Assert
        Assert.IsTrue(museum.FileExists(filePath));
        Assert.AreEqual("[]", museum.ReadAllText(filePath));
    }
}