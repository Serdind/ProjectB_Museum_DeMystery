using NUnit.Framework.Internal;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;

namespace UnitTests;

[TestClass]
public class PersonTests
{
    [TestMethod]
    public void LoginTest()
    {
        // Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;

        string filePath1 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath1] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""TestGuide"",
                ""QR"": ""4892579""
            }
        ]
        ";
        string filePath2 = Model<Visitor>.GetFileNameVisitors();

        museum.Files[filePath2] = @"
        [
            {
                ""Id"": 1,
                ""QR"": ""523523"",
                ""TourId"": 1
            }
        ]
        ";

        string filePath3 = Model<DepartmentHead>.GetFileNameAdmins();

        museum.Files[filePath3] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""TestAdmin"",
                ""QR"": ""6457823""
            }
        ]
        ";

        Person person = new Person(null);

        //Act
        string roleGuide = person.Login("4892579");
        string roleVisitor = person.Login("523523");
        string roleAdmin = person.Login("6457823");

        //Assert
        Assert.AreEqual("Guide", roleGuide);
        Assert.AreEqual("Visitor", roleVisitor);
        Assert.AreEqual("Admin", roleAdmin);
    }

    [TestMethod]
    public void AccCreatedTest()
    {
        //Arrange
        FakeMuseum museum = new FakeMuseum();
        Program.Museum = museum;
        
        string filePath1 = Model<Guide>.GetFileNameGuides();

        museum.Files[filePath1] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""TestGuide"",
                ""QR"": ""4892579""
            }
        ]
        ";

        string filePath2 = Model<DepartmentHead>.GetFileNameAdmins();

        museum.Files[filePath2] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""TestAdmin"",
                ""QR"": ""6457823""
            }
        ]
        ";

        string filePath3 = Model<UniqueCodes>.GetFileNameUniqueCodes();

        museum.Files[filePath3] = @"
        [
            ""139278"",
            ""8752316"",
            ""124678""
        ]
        ";

        Person person = new Person(null);
        
        //Act

        //ValidCodes
        bool result1 = person.AccCreated("139278");
        bool result2 = person.AccCreated("8752316");
        bool result3 = person.AccCreated("124678");

        //InvalidCode
        bool result4 = person.AccCreated("678142");

        //GuideCode
        bool result5 = person.AccCreated("4892579");

        //AdminCode
        bool result6 = person.AccCreated("6457823");

        //Assert
        Assert.IsTrue(result1);
        Assert.IsTrue(result2);
        Assert.IsTrue(result3);

        Assert.IsFalse(result4);
        Assert.IsFalse(result5);
        Assert.IsFalse(result6);
    }
}