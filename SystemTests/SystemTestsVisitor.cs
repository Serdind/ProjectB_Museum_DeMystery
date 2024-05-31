using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

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

            string filePath1 = Model<GuidedTour>.GetFileNameTours();

            museum.Files[filePath1] = @"
            [
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
            ]
            ";

            string filePath2 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath2] = @"
            [
                ""139278"",
                ""78643"",
                ""124678""
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "y", "y", "y", "y", "y", "y", "y", "y", // Intro screens
                "e",  // Select language
                "l",  // Login
                "78643",  // QR code input
                "r", // Make reservation input
                "1", // Tour id input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string message = $"Reservation successful. You have reserved the following tour:\n" +
                        $"Date: 25-5-2024\n" +
                        $"Time: 10:00\n" +
                        $"Duration: 40 min\n" +
                        $"Language: English\n";

            string writtenLines = museum.GetWrittenLinesAsString();
            Assert.IsTrue(writtenLines.Contains("Cancel reservation(C)"));
        }

        [TestMethod]
        public void VistorLoginAndViewReservationMadeTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();
            Program.Museum = museum;

            string filePath1 = Model<GuidedTour>.GetFileNameTours();

            museum.Files[filePath1] = @"
            [
                {
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""2024-05-25T10:00:00"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
                }
            ]
            ";

            string filePath2 = Model<UniqueCodes>.GetFileNameUniqueCodes();

            museum.Files[filePath2] = @"
            [
                ""139278"",
                ""78643"",
                ""124678""
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "y", "y", "y", "y", "y", "y", "y", "y", // Intro screens
                "e",  // Select language
                "l",  // Login
                "124678",  // QR code input
                "r", // Make reservation input
                "1", // Tour id input
                "m", // My reservations input
                "l" // Log out input

            };

            // Act
            ProgramController.Start();

            // Assert
            string message = $"Date: 25-5-2024\n" +
                        $"Time: 10:00\n" +
                        $"Duration: 40 min\n" +
                        $"Language: English\n";

            string writtenLines = museum.GetWrittenLinesAsString();
            Assert.IsTrue(writtenLines.Contains(message));
        }

        [TestMethod]
        public void VistorLoginAndCancelReservationTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();
            TestableProgramController testableProgramController = new TestableProgramController(museum);

            string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
            string fileName1 = "toursTest.json";
            string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

            museum.Files[filePath1] = @"
            [
                {
                ""ID"": ""1"",
                ""Name"": ""Tour 1"",
                ""Date"": ""2024-05-25T10:00:00"",
                ""MaxParticipants"": 13,
                ""ReservedVisitors"": [],
                ""Language"": ""English"",
                ""Status"": true
                }
            ]
            ";

            string subdirectory2 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
            string fileName2 = "unique_codesTest.json";
            string userDirectory2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath2 = Path.Combine(userDirectory2, subdirectory2, fileName2);

            museum.Files[filePath2] = @"
            [
                ""139278"",
                ""78643"",
                ""124678""
            ]
            ";

            museum.LinesToRead = new List<string>
            {
                "y", "y", "y", "y", "y", "y", "y", "y", // Intro screens
                "e",  // Select language
                "l",  // Login
                "124678",  // QR code input
                "r", // Make reservation input
                "1", // Tour id input
                "c", // Cancel reservation input
                "y", // Confirm cancellation input
                "l" // Log out input

            };

            // Act
            testableProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Assert.IsTrue(writtenLines.Contains("Reservation cancelled successfully."));
        }
    }
}