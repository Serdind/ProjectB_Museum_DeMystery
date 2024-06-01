using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

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

            string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";

            string guidesFilePath = Path.Combine(baseDirectory, subdirectory, "guidesTest.json");
            string toursFilePath = Path.Combine(baseDirectory, subdirectory, "toursTest.json");
            string uniqueCodesFilePath = Path.Combine(baseDirectory, subdirectory, "unique_codesTest.json");

            museum.Files[guidesFilePath] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestGuide1"",
                    ""QR"": ""7789423""
                }
            ]
            ";

           museum.Files[toursFilePath] = @"
            [
                {
                    ""ID"": 1,
                    ""Name"": ""Museum tour"",
                    ""Date"": ""2024-05-11T11:30:00"",
                    ""Language"": ""English"",
                    ""NameGuide"": ""TestGuide1"",
                    ""ReservedVisitors"": [],
                    ""Status"": true,
                    ""MaxParticipants"": 13
                },
                {
                    ""ID"": 2,
                    ""Name"": ""Another tour"",
                    ""Date"": ""2024-05-11T15:00:00"",
                    ""Language"": ""English"",
                    ""NameGuide"": ""TestGuide1"",
                    ""ReservedVisitors"": [],
                    ""Status"": true,
                    ""MaxParticipants"": 13
                }
                ]";
            

            museum.Files[uniqueCodesFilePath] = @"
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
                "7789423",  // QR code input
                "m", // My tours input
                "v", // View visitors input
                "1", // Tour id input
                "a", // Add visitor input
                "78643", // Visitor qr input
                "b", // Back input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Assert.IsTrue(writtenLines.Contains("Succesfully added visitor to tour."));
        }

        [TestMethod]
        public void GuideLoginAndRemoveVisitorToTourTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();

            string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";

            string guidesFilePath = Path.Combine(baseDirectory, subdirectory, "guidesTest.json");
            string toursFilePath = Path.Combine(baseDirectory, subdirectory, "toursTest.json");
            string uniqueCodesFilePath = Path.Combine(baseDirectory, subdirectory, "unique_codesTest.json");
            string visitorsFilePath = Path.Combine(baseDirectory, subdirectory, "visitorsTest.json");

            museum.Files[visitorsFilePath] = @"
            [
                {
                    ""Id"": 1,
                    ""TourId"": 1,
                    ""QR"": ""78643""
                }
            ]
            ";

            museum.Files[guidesFilePath] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestGuide"",
                    ""QR"": ""6457823""
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
                    ""ReservedVisitors"": [
                        {
                    ""Id"": 1,
                    ""TourId"": 1,
                    ""QR"": ""78643""
                }
                    ],
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

            museum.Files[uniqueCodesFilePath] = @"
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
                "6457823",  // QR code input
                "m", // My tours input
                "v", // View visitors input
                "1", // Tour id input
                "r", // Remove visitor input
                "78643", // Visitor qr input
                "b", // Back input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Assert.IsTrue(writtenLines.Contains("Succesfully removed visitor from tour."));
        }

        [TestMethod]
        public void GuideLoginAndStartTourTest()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();

            string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";

            string guidesFilePath = Path.Combine(baseDirectory, subdirectory, "guidesTest.json");
            string toursFilePath = Path.Combine(baseDirectory, subdirectory, "toursTest.json");
            string uniqueCodesFilePath = Path.Combine(baseDirectory, subdirectory, "unique_codesTest.json");
            string visitorsFilePath = Path.Combine(baseDirectory, subdirectory, "visitorsTest.json");

            museum.Files[visitorsFilePath] = @"
            [
                {
                    ""Id"": 1,
                    ""TourId"": 1,
                    ""QR"": ""78643""
                }
            ]
            ";

            museum.Files[guidesFilePath] = @"
            [
                {
                    ""Id"": ""1"",
                    ""Name"": ""TestGuide"",
                    ""QR"": ""6457823""
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
                    ""ReservedVisitors"": [
                        {
                    ""Id"": 1,
                    ""TourId"": 1,
                    ""QR"": ""78643""
                }
                    ],
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

            museum.Files[uniqueCodesFilePath] = @"
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
                "6457823",  // QR code input
                "m", // My tours input
                "s", // Start tour input
                "1", // Tour id input
                "b", // Back input
                "b", // Back input
                "l" // Log out input
            };

            // Act
            ProgramController.Start();

            // Assert
            string writtenLines = museum.GetWrittenLinesAsString();
            Assert.IsTrue(writtenLines.Contains(@"Tour has been started:
                    Date: 25-5-2024
                    Time: 10:00
                    Language: English
                    Guide: TestGuide"));
        }
    }
}