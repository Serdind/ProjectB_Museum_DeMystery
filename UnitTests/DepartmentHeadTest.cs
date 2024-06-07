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
            // Arrange
            string name = "TestName";
            string qr = "123456";

            // Act
            DepartmentHead departmentHead = new DepartmentHead(name, qr);

            // asert
            Assert.AreEqual(1, departmentHead.Id);
        }
    }
}