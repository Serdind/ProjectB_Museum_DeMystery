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
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = "[]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
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
                            $"Duration: 40 minutes\n" +
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
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = "[]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
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
                        $"Duration: 40 minutes\n" +
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
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = @"
            [
                {
                    ""Id"": ""1"",
                    ""TourId"": ""1"",
                    ""QR"": ""8752316""
                }
            ]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
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
        public void VistorLoginAndCancelReservationOptionTest()
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
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = @"
            [
                {
                    ""Id"": ""1"",
                    ""TourId"": ""1"",
                    ""QR"": ""8752316""
                }
            ]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
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
            Assert.IsTrue(writtenLines.Contains("Cancel reservation(C)"));
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
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = @"
            [
                {
                    ""Id"": ""1"",
                    ""TourId"": ""1"",
                    ""QR"": ""8752316""
                }
            ]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
                "n", // No help needed
                "c", // Cancel reservation
                "n", // No input
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
                        $"Duration: 40 minutes\n" +
                        $"Language: English\n" +
                        $"Name of guide: TestGuide\n";

            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains(message));
        }

        [TestMethod]
        public void ValidBarcodeTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();
            Program.Museum = museum;

            string filePath2 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath2] = @"
            [
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = "[]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
                "n", // No help needed
                "f", // Finish
                "y" // Yes input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Do you want information about how to use this application first?"));
        }

        [TestMethod]
        public void InvalidBarcodeTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();
            Program.Museum = museum;

            string filePath2 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath2] = @"
            [
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = "[]";

            museum.LinesToRead = new List<string>
            {
                "99999",  // QR code input
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
        public void OptionInstructionsTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();
            Program.Museum = museum;

            string filePath2 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath2] = @"
            [
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = "[]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
                "n", // No instructions input
                "h", // Instructions input
                "f", // Finish input
                "y" // Yes input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Help(H)"));
        }

        [TestMethod]
        public void VistorLoginAndGoOutOfMakingReservationTest()
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
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = "[]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
                "n", // No help needed
                "r", // Make reservation
                "b", // Back input
                "f", // Finish
                "y" // Yes input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Insert (Back or B) if you want to go back"));
        }

        [TestMethod]
        public void VistorLoginAndCheckTimeTest()
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
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = @"[]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
                "n", // No help needed
                "r", // Make reservation input
                "b", // No input
                "f", // Finish input
                "y" // Yes input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Duration"));
        }

        [TestMethod]
        public void VistorLoginAndCheckInformationTest()
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
                ""8752316""
            ]
            ";

            string filePath3 = Model<Visitor>.GetFileNameVisitors();

            museum.Files[filePath3] = @"[]";

            museum.LinesToRead = new List<string>
            {
                "8752316",  // QR code input
                "n", // No help needed
                "r", // Make reservation input
                "b", // No input
                "f", // Finish input
                "y" // Yes input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Debug.WriteLine(writtenLines);
            Assert.IsTrue(writtenLines.Contains("Date"));
            Assert.IsTrue(writtenLines.Contains("Time"));
            Assert.IsTrue(writtenLines.Contains("Duration"));
            Assert.IsTrue(writtenLines.Contains("Language"));
            Assert.IsTrue(writtenLines.Contains("Guide"));
            Assert.IsTrue(writtenLines.Contains("Remaining spots"));
        }
    }
}