using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;

namespace SystemTests
{
    [TestClass]
    public class SystemTestsAdmin
    {
        [TestMethod]
        public void AdminLoginAndOverviewToursTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();

            string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";

            string adminsFilePath = Path.Combine(baseDirectory, subdirectory, "adminsTest.json");
            string toursFilePath = Path.Combine(baseDirectory, subdirectory, "toursTest.json");

            museum.Files[adminsFilePath] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            museum.Files[toursFilePath] = @"
            [
                {
                    ""ID"": ""1"",
                    ""Name"": ""Tour 1"",
                    ""Date"": ""2024-05-25T10:00:00"",
                    ""MaxParticipants"": 13,
                    ""ReservedVisitors"": [],
                    ""NameGuide"": ""TestGuide"",
                    ""Language"": ""English"",
                    ""Status"": true
                },
                {
                    ""ID"": ""2"",
                    ""Name"": ""Tour 2"",
                    ""Date"": ""2024-05-25T14:00:00"",
                    ""MaxParticipants"": 13,
                    ""ReservedVisitors"": [],
                    ""NameGuide"": ""TestGuide"",
                    ""Language"": ""English"",
                    ""Status"": true
                }
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "y", "y", "y", "y", "y", "y", "y", "y", // Intro screens
                "e",  // Select language
                "l",  // Login
                "897324",  // QR code input
                "t", // Tours input
                "l"
            };

            string json = museum.ReadAllText(toursFilePath);
            var tours = JsonConvert.DeserializeObject<List<GuidedTour>>(json);
            
            var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Date");
                table.AddColumn("Time");
                table.AddColumn("Duration");
                table.AddColumn("Language");
                table.AddColumn("Guide");
                table.AddColumn("Remaining spots");
                table.AddColumn("Status");

                foreach (var tour in tours)
                {
                    string timeOnly = tour.Date.ToString("HH:mm");
                    string dateOnly = tour.Date.ToShortDateString();
                    int remainingSpots = tour.MaxParticipants - tour.ReservedVisitors.Count;
                    string status = tour.Status ? "Active" : "Inactive";

                    table.AddRow(
                        tour.ID.ToString(),
                        dateOnly,
                        timeOnly,
                        "40 minutes",
                        tour.Language,
                        "TestGuide",
                        remainingSpots.ToString(),
                        status
                    );
                }

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Assert.IsTrue(writtenLines.Contains(table.ToString()));
        }

        [TestMethod]
        public void AdminLoginAndAddTourTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();

            string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";

            string adminsFilePath = Path.Combine(baseDirectory, subdirectory, "adminsTest.json");
            string toursFilePath = Path.Combine(baseDirectory, subdirectory, "toursTest.json");

            museum.Files[adminsFilePath] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            museum.Files[toursFilePath] = @"
            [
                {
                    ""ID"": ""1"",
                    ""Name"": ""Tour 1"",
                    ""Date"": ""2024-05-25T10:00:00"",
                    ""MaxParticipants"": 13,
                    ""ReservedVisitors"": [],
                    ""NameGuide"": ""TestGuide"",
                    ""Language"": ""English"",
                    ""Status"": true
                },
                {
                    ""ID"": ""2"",
                    ""Name"": ""Tour 2"",
                    ""Date"": ""2024-05-25T14:00:00"",
                    ""MaxParticipants"": 13,
                    ""ReservedVisitors"": [],
                    ""NameGuide"": ""TestGuide"",
                    ""Language"": ""English"",
                    ""Status"": true
                }
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "y", "y", "y", "y", "y", "y", "y", "y", // Intro screens
                "e",  // Select language
                "l",  // Login
                "897324",  // QR code input
                "a", // Tours input
                "TestName", // Name input
                "25-5-2024 19:00", // Date input
                "Dutch", // Language input
                "l" // Log out input


            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Assert.IsTrue(writtenLines.Contains("Tour succesfully added."));
        }

        [TestMethod]
        public void AdminLoginAndEditTourTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();

            string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";

            string adminsFilePath = Path.Combine(baseDirectory, subdirectory, "adminsTest.json");
            string toursFilePath = Path.Combine(baseDirectory, subdirectory, "toursTest.json");

            museum.Files[adminsFilePath] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestAdmin"",
                    ""QR"": ""897324""
                }
            ]
            ";

            museum.Files[toursFilePath] = @"
            [
                {
                    ""ID"": ""1"",
                    ""Name"": ""Tour 1"",
                    ""Date"": ""2024-05-25T10:00:00"",
                    ""MaxParticipants"": 13,
                    ""ReservedVisitors"": [],
                    ""NameGuide"": ""TestGuide"",
                    ""Language"": ""English"",
                    ""Status"": true
                },
                {
                    ""ID"": ""2"",
                    ""Name"": ""Tour 2"",
                    ""Date"": ""2024-05-25T14:00:00"",
                    ""MaxParticipants"": 13,
                    ""ReservedVisitors"": [],
                    ""NameGuide"": ""TestGuide"",
                    ""Language"": ""English"",
                    ""Status"": true
                }
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "y", "y", "y", "y", "y", "y", "y", "y", // Intro screens
                "e",  // Select language
                "l",  // Login
                "897324",  // QR code input
                "e", // Edit tours input
                "2", // Tour id input
                "n", // Name change input
                "TestName", // New name input
                "l" // Log out input


            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Assert.IsTrue(writtenLines.Contains("Name set to TestName"));
        }
    }
}