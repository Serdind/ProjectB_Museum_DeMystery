using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SystemTests
{
    [TestClass]
    public class SystemTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            FakeMuseum museum = new FakeMuseum();
            TestableProgramController testableProgramController = new TestableProgramController(museum);

            museum.LinesToRead = new List<string>
            {
                "y", "y", "y", "y", "y", "y", "y", "y", // Intro screens
                "e",  // Select language
                "l",  // Login
                "139278"  // QR code input
            };

            // Act
            testableProgramController.Start();

            // Assert
            Assert.IsTrue(museum.LinesWritten.Contains("Code is not valid."));
        }
    }
}