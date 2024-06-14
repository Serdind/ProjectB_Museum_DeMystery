using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;

namespace SystemTests
{
    [TestClass]
    public class SystemTestsGuide
    {
        [TestMethod]
        public void GuideLoginAndAddVisitorToTourTest()
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
                    ""QR"": ""4892579""
                }
            ]
            ";

            string filePath3 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath3] = @"
            [
                ""8752316"",
                ""122718""
            ]
            ";

            string filePath4 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath4] = "[]";

            museum.LinesToRead = new List<string>
            {
                "4892579",  // Barcode input
                "v", // View visitors input
                "1", // Tour id input
                "a", // Add visitor input
                "8752316", // Visitor barcode input
                "122718", // Visitor barcode input
                "b", // Back input
                "b", // Back input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Succesfully added visitor to tour."));

            var visitors = JsonConvert.DeserializeObject<List<Visitor>>(museum.Files[filePath4]);

            Assert.AreEqual(1, visitors[0].Id);
            Assert.AreEqual(1, visitors[0].TourId);
            Assert.AreEqual("8752316", visitors[0].QR);
        }
        
        [TestMethod]
        public void GuideLoginAndRemoveVisitorToTourTest()
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
                            ""QR"": ""8752316""
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
                    ""QR"": ""4892579""
                }
            ]
            ";

            string filePath3 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath3] = @"
            [
                ""8752316""
            ]
            ";

            string filePath4 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath4] = @"
            [
                {
                    ""Id"": ""1"",
                    ""TourId"": ""1"",
                    ""QR"": ""8752316""
                }
            ]";

            museum.LinesToRead = new List<string>
            {
                "4892579",  // Barcode input
                "v", // View visitors input
                "1", // Tour id input
                "r", // Remove visitor input
                "8752316", // Visitor barcode input
                "b", // Back input
                "b", // Back input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Succesfully removed visitor from tour."));

            var visitors = JsonConvert.DeserializeObject<List<Visitor>>(museum.Files[filePath4]);

            Assert.IsFalse(visitors.Any());
        }

        [TestMethod]
        public void GuideLoginAndStartTourTest()
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
                            ""QR"": ""8752316""
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
                    ""QR"": ""4892579""
                }
            ]
            ";

            string filePath3 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath3] = @"
            [
                ""8752316""
            ]
            ";

            string filePath4 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath4] = @"
            [
                {
                    ""Id"": ""1"",
                    ""TourId"": ""1"",
                    ""QR"": ""8752316""
                }
            ]";

            museum.LinesToRead = new List<string>
            {
                "4892579",  // Barcode input
                "s", // Start tour input
                "1", // Tour id input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string message = $"Tour has been started:\n" +
                        $"Date: {currentDate.ToShortDateString()}\n" +
                        $"Duration: {currentDate.ToShortTimeString()}\n" +
                        $"Language: English\n" +
                        $"Name of guide: TestGuide\n";

            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains(message));
        }

        [TestMethod]
        public void GuideLoginAndGoOutOfViewVisitorsTest()
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

            string filePath2 = Model<Guide>.GetFileNameGuides();

            museum.Files[filePath2] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestGuide"",
                    ""QR"": ""4892579""
                }
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = "[]";

            museum.LinesToRead = new List<string>
            {
                "4892579",  // Barcode input
                "v", // View visitors input
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