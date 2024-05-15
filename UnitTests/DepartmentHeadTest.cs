using NUnit.Framework.Internal;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class DepartmentHeadTests
    {
        [TestMethod]
        public void DepartmentHeadInitializationTest()
        {
            // okee Arrange
            string name = "TestName";
            string qr = "123456";

            // Act
            DepartmentHead departmentHead = new DepartmentHead(name, qr);

            // asert
            Assert.IsNotNull(departmentHead);
            Assert.AreEqual(name, departmentHead.Name);
            Assert.AreEqual(1, departmentHead.Id);
        }

        [TestMethod]
        public void DepartmentHeadIdIncrementTest()
        {
            // Arrange
            string name1 = "TestName1";
            string qr1 = "123456";
            string name2 = "TestName2";
            string qr2 = "654321";

            // Act
            DepartmentHead departmentHead1 = new DepartmentHead(name1, qr1);
            DepartmentHead departmentHead2 = new DepartmentHead(name2, qr2);

            // Assert
            Assert.AreEqual(1, departmentHead1.Id); 
            Assert.AreEqual(2, departmentHead2.Id); 
        }
    }
}