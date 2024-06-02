using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;

namespace SystemTests
{
    [TestClass]
    public class SystemTestsAdmin
    {
        [TestMethod]
        public void AdminLoginAndOverviewToursTest()
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
                currentDate.ToShortDateString(), // Date input
                "l" // Log out input

            };

            // Act
            ProgramController.Start();

            string json = museum.ReadAllText(filePath1);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);

            var expectedTable = new Table().Border(TableBorder.Rounded);
            expectedTable.AddColumn("ID");
            expectedTable.AddColumn("Date");
            expectedTable.AddColumn("Time");
            expectedTable.AddColumn("Duration");
            expectedTable.AddColumn("Language");
            expectedTable.AddColumn("Guide");
            expectedTable.AddColumn("Remaining spots");
            expectedTable.AddColumn("Status");

            foreach (var tour in tours)
            {
                expectedTable.AddRow(
                    tour.ID.ToString(),
                    tour.Date.ToShortDateString(),
                    tour.Date.ToString("HH:mm"),
                    "40 minutes",
                    tour.Language,
                    "TestGuide",
                    (tour.MaxParticipants - tour.ReservedVisitors.Count).ToString(),
                    tour.Status ? "Active" : "Inactive"
                );
            }

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains(expectedTable.ToString()));
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
        public void AdminLoginAndAdminMenuTest()
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
    }
}