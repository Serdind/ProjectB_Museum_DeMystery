using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace SystemTests
{
    [TestClass]
    public class SystemTestsVisitor
    {
        [TestMethod]
        public void VistorLoginAndMakeReservationTest()
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
                "n", // No help needed
                "r", // Make reservation
                "1", // Tour ID input
                "f" // Finish
            };

            // Act
            ProgramController.Start();

            // Assert
            string dateOnly = currentDate.ToString("d");
            string timeOnly = currentDate.ToString("HH:mm");
            string message = $"Reservation successful. You have reserved the following tour:\n" +
                            $"Date: {dateOnly}\n" +
                            $"Time: {timeOnly}\n" +
                            $"Duration: 40 min\n" +
                            $"Language: English\n";

            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains(message));
        }
        [TestMethod]
        public void VistorLoginAndViewReservationMadeTest()
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
                "n", // No help needed
                "r", // Make reservation
                "1", // Tour ID input
                "m", // My tour
                "f" // Finish input
            };

            // Act
            ProgramController.Start();

            // Assert
            string dateOnly = currentDate.ToString("d");
            string timeOnly = currentDate.ToString("HH:mm");
            string message = $"Date: {dateOnly}\n" +
                        $"Time: {timeOnly}\n" +
                        $"Duration: 40 min\n" +
                        $"Language: English\n" +
                        $"Name of guide: TestGuide\n";

            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains(message));
        }

        [TestMethod]
        public void VistorLoginAndCancelReservationTest()
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

            string filePath2 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath2] = @"
            [
                ""139278"",
                ""78643"",
                ""124678""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = @"
            [
                {
                    ""Id"": ""1"",
                    ""TourId"": ""1"",
                    ""QR"": ""78643""
                }
            ]";

            museum.LinesToRead = new List<string>
            {
                "78643",  // QR code input
                "n", // No help needed
                "c", // Cancel reservation
                "y", // Yes input
                "f", // Finish input
                "y" // Yes input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Reservation cancelled successfully."));
        }

        [TestMethod]
        public void VistorLoginAndCancelReservationAndThenMakeReservationTest()
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

            string filePath2 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath2] = @"
            [
                ""139278"",
                ""78643"",
                ""124678""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = @"
            [
                {
                    ""Id"": ""1"",
                    ""TourId"": ""1"",
                    ""QR"": ""78643""
                }
            ]";

            museum.LinesToRead = new List<string>
            {
                "78643",  // QR code input
                "n", // No help needed
                "c", // Cancel reservation
                "y", // Yes input
                "r", // Make reservation input
                "1", // Tour id input
                "f", // Finish input
            };

            // Act
            ProgramController.Start();

            // Assert
            string dateOnly = currentDate.ToString("d");
            string timeOnly = currentDate.ToString("HH:mm");
            string message = $"Date: {dateOnly}\n" +
                        $"Time: {timeOnly}\n" +
                        $"Duration: 40 min\n" +
                        $"Language: English\n" +
                        $"Name of guide: TestGuide\n";

            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains(message));
        }

        [TestMethod]
        public void ValidQrTest()
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
                "n", // No help needed
                "f", // Finish
                "y" // Yes input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Make reservation(R)"));
        }

        [TestMethod]
        public void InvalidQrTest()
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
                "235325"  // QR code input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Code is not valid."));
        }
    }
}