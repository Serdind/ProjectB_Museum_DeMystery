using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;
using System.Text;

namespace SystemTests
{
    [TestClass]
    public class SystemTestsAdmin
    {
        [TestMethod]
        public void AdminLoginAndOverviewToursTodayTest()
        {
            FakeMuseum museum = new FakeMuseum();
            Program.Museum = museum;

            DateTime currentDate = DateTime.Today.AddHours(23).AddMinutes(59);

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

            string filePath2 = Model<DepartmentHead>.GetFileNameAdmins();

            museum.Files[filePath2] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "897324",  // QR code input
                "t", // Overview tours input
                "t", // Tours from today input
                "l" // Log out input

            };

            // Act
            ProgramController.Start();
            
            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);

            Assert.IsTrue(writtenLines.Contains($"{currentDate.ToString("d-M-yyyy")}"));
        }

        [TestMethod]
        public void AdminLoginAndOverviewToursAfterTomorrowTest()
        {
            FakeMuseum museum = new FakeMuseum();
            Program.Museum = museum;

            DateTime dateTomorrow = DateTime.Today.AddDays(1).AddHours(23).AddMinutes(59);

            string dateTomorrowString = dateTomorrow.ToString("yyyy-MM-ddTHH:mm:ss");

            string filePath1 = Model<GuidedTour>.GetFileNameTours();

            string toursJson = $@"
            [
                {{
                    ""ID"": ""1"",
                    ""Date"": ""{dateTomorrowString}"",
                    ""NameGuide"": ""TestGuide"",
                    ""MaxParticipants"": 13,
                    ""ReservedVisitors"": [],
                    ""Language"": ""English"",
                    ""Status"": true
                }}
            ]
            ";

            museum.Files[filePath1] = toursJson;

            string filePath2 = Model<DepartmentHead>.GetFileNameAdmins();

            museum.Files[filePath2] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "897324",  // QR code input
                "t", // Overview tours input
                "a", // Tours after today input
                "l" // Log out input

            };

            // Act
            ProgramController.Start();
            
            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);

            Assert.IsTrue(writtenLines.Contains($"{dateTomorrow.ToString("d-M-yyyy")}"));
        }

        [TestMethod]
        public void AdminLoginAndAddTourTest()
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

            string filePath2 = Model<DepartmentHead>.GetFileNameAdmins();

            museum.Files[filePath2] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "897324",  // QR code input
                "a", // Tours input
                "20:00", // Time input
                "Dutch", // Language input
                "y", // Yes input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Tour succesfully added."));

            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(museum.Files[filePath1]);

            Assert.AreEqual("20:00", tours[1].Date.ToString("HH:mm"));
            Assert.AreEqual("Dutch", tours[1].Language);
        }

        [TestMethod]
        public void AdminLoginAndEditStatusTourTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();
            Program.Museum = museum;

            DateTime currentDate = DateTime.Today;
            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

            string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");
            string futureTourDateString = currentDate.AddDays(+1).ToString("yyyy-MM-ddTHH:mm:ss");

            DateTime currentDateTime = DateTime.ParseExact(currentDateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            

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
                }},
                {{
                    ""ID"": ""2"",
                    ""Date"": ""{futureTourDateString}"",
                    ""NameGuide"": ""TestGuide"",
                    ""MaxParticipants"": 13,
                    ""ReservedVisitors"": [],
                    ""Language"": ""English"",
                    ""Status"": true
                }}
            ]
            ";

            museum.Files[filePath1] = toursJson;

            string filePath2 = Model<DepartmentHead>.GetFileNameAdmins();

            museum.Files[filePath2] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "897324",  // QR code input
                "e", // Tours input
                currentDateTime.ToString("HH:mm"), // Date input
                "s", // Status changed to inactive input
                "b", // Back input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Status set to inactive"));

            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(museum.Files[filePath1]);

            Assert.AreEqual(false, tours[0].Status);
            Assert.AreEqual(false, tours[0].Status);
        }

        [TestMethod]
        public void AdminLoginAndEditTourTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();
            Program.Museum = museum;

            DateTime currentDate = DateTime.Today;
            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 0);

            string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");
            string futureTourDateString = currentDate.AddDays(+1).ToString("yyyy-MM-ddTHH:mm:ss");

            DateTime currentDateTime = DateTime.ParseExact(currentDateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            

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
                }},
                {{
                    ""ID"": ""2"",
                    ""Date"": ""{futureTourDateString}"",
                    ""NameGuide"": ""TestGuide"",
                    ""MaxParticipants"": 13,
                    ""ReservedVisitors"": [],
                    ""Language"": ""English"",
                    ""Status"": true
                }}
            ]
            ";

            museum.Files[filePath1] = toursJson;

            string filePath2 = Model<DepartmentHead>.GetFileNameAdmins();

            museum.Files[filePath2] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "897324",  // QR code input
                "e", // Tours input
                currentDateTime.ToString("HH:mm"), // Date input
                "l", // Language change input
                "Dutch", // New language input
                "b", // Back input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert            
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Language set to Dutch"));

            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(museum.Files[filePath1]);
            
            Assert.AreEqual("Dutch", tours[0].Language);
            Assert.AreEqual("Dutch", tours[0].Language);
        }

        [TestMethod]
        public void AdminLoginAndAdminMenuTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();
            Program.Museum = museum;

            string filePath2 = Model<DepartmentHead>.GetFileNameAdmins();

            museum.Files[filePath2] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "897324",  // QR code input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Overview tours(T)\nAdd tour (A)\nEdit tour (E)\nLog out (L)"));
        }

        [TestMethod]
        public void AdminLoginAndGoOutOfAddTourTest()
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
                    ""ReservedVisitors"": [],
                    ""Language"": ""English"",
                    ""Status"": true
                }
            ]";

            museum.Files[filePath1] = toursJson;

            string filePath2 = Model<DepartmentHead>.GetFileNameAdmins();

            museum.Files[filePath2] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = "[]";

            museum.LinesToRead = new List<string>
            {
                "897324",  // QR code input
                "a", // View visitors input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Insert (Back or B) if you want to go back"));
        }
    }
}