using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class GuidedTourTests
    {
        [TestMethod]
        public void GuidedTourInitializationTest()
        {
            // Arrange
            DateTime date = DateTime.Now;
            string language = "English";
            string nameGuide = "TestGuide";

            // Act
            GuidedTour guidedTour = new GuidedTour(date, language, nameGuide);

            // Assert
            Assert.IsNotNull(guidedTour);
            Assert.AreEqual(date, guidedTour.Date);
            Assert.AreEqual(language, guidedTour.Language);
            Assert.AreEqual(nameGuide, guidedTour.NameGuide);
            Assert.IsNotNull(guidedTour.ReservedVisitors);
            Assert.IsTrue(guidedTour.Status);
        }

        [TestMethod]
        public void GuidedTourIdIncrementTest()
        {
            // Arrange
            DateTime date1 = DateTime.Now;
            string language1 = "English";
            string nameGuide1 = "TestGuide1";
            DateTime date2 = DateTime.Now;
            string language2 = "Spanish";
            string nameGuide2 = "TestGuide2";

            // Act
            GuidedTour guidedTour1 = new GuidedTour(date1, language1, nameGuide1);
            GuidedTour guidedTour2 = new GuidedTour(date2, language2, nameGuide2);

            // Assert
            Assert.AreEqual(1, guidedTour1.ID); 
            Assert.AreEqual(2, guidedTour2.ID); 
        }

        [TestMethod]
        public void GuidedTourReservedVisitorsInitializationTest()
        {
            // Arrange
            DateTime date = DateTime.Now;
            string language = "English";
            string nameGuide = "TestGuide";

            // Act
            GuidedTour guidedTour = new GuidedTour(date, language, nameGuide);

            // Assert
            Assert.IsNotNull(guidedTour.ReservedVisitors);
            Assert.AreEqual(0, guidedTour.ReservedVisitors.Count);
        }
    }
}