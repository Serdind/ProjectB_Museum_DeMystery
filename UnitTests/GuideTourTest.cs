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
            // Arrange Weet niet zeker of deze goed is 
            string name = "TestTour";
            DateTime date = DateTime.Now;
            string language = "English";
            string nameGuide = "TestGuide";

            // Act
            GuidedTour guidedTour = new GuidedTour(name, date, language, nameGuide);

            // Assert
            Assert.IsNotNull(guidedTour);
            Assert.AreEqual(name, guidedTour.Name);
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
            string name1 = "TestTour1";
            DateTime date1 = DateTime.Now;
            string language1 = "English";
            string nameGuide1 = "TestGuide1";
            string name2 = "TestTour2";
            DateTime date2 = DateTime.Now;
            string language2 = "Spanish";
            string nameGuide2 = "TestGuide2";

            // Act
            GuidedTour guidedTour1 = new GuidedTour(name1, date1, language1, nameGuide1);
            GuidedTour guidedTour2 = new GuidedTour(name2, date2, language2, nameGuide2);

            // Assert
            Assert.AreEqual(1, guidedTour1.ID); 
            Assert.AreEqual(2, guidedTour2.ID); 
        }

        [TestMethod]
        public void GuidedTourReservedVisitorsInitializationTest()
        {
            // Arrange
            string name = "TestTour";
            DateTime date = DateTime.Now;
            string language = "English";
            string nameGuide = "TestGuide";

            // Act
            GuidedTour guidedTour = new GuidedTour(name, date, language, nameGuide);

            // Assert
            Assert.IsNotNull(guidedTour.ReservedVisitors);
            Assert.AreEqual(0, guidedTour.ReservedVisitors.Count);
        }
    }
}