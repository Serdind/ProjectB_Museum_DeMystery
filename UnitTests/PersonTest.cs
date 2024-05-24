using NUnit.Framework.Internal;

namespace UnitTests;

[TestClass]
public class PersonTests
{
    [TestMethod]
    public void LoginTest()
    {
        //Arrange
        FakeMuseum museum = new FakeMuseum();
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "guidesTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName1 = "visitorsTest.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

        string subdirectory2 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName2 = "adminsTest.json";
        string userDirectory2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath2 = Path.Combine(userDirectory2, subdirectory2, fileName2);
        
        museum.Files[filePath] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""TestGuide"",
                ""QR"": ""4892579""
            }
        ]
        ";
        museum.Files[filePath1] = @"
        [
            {
                ""Id"": 1,
                ""QR"": ""523523"",
                ""TourId"": 1
            }
        ]
        ";
        museum.Files[filePath2] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""TestAdmin"",
                ""QR"": ""6457823""
            }
        ]
        ";
        TestablePerson fakePerson = new TestablePerson(museum);

        //Act
        string roleGuide = fakePerson.Login("4892579");
        string roleVisitor = fakePerson.Login("523523");
        string roleAdmin = fakePerson.Login("6457823");

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
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName = "guidesTest.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName1 = "adminsTest.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

        string subdirectory2 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
        string fileName2 = "unique_codesTest.json";
        string userDirectory2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath2 = Path.Combine(userDirectory2, subdirectory2, fileName2);
        
        museum.Files[filePath] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""TestGuide"",
                ""QR"": ""4892579""
            }
        ]
        ";
        museum.Files[filePath1] = @"
        [
            {
                ""ID"": 1,
                ""Name"": ""TestAdmin"",
                ""QR"": ""6457823""
            }
        ]
        ";
        museum.Files[filePath2] = @"
        [
            ""139278"",
            ""78643"",
            ""124678""
        ]
        ";
        
        TestablePerson fakePerson = new TestablePerson(museum);

        //Act

        //ValidCodes
        bool result1 = fakePerson.AccCreated("139278");
        bool result2 = fakePerson.AccCreated("78643");
        bool result3 = fakePerson.AccCreated("124678");

        //InvalidCode
        bool result4 = fakePerson.AccCreated("678142");

        //GuideCode
        bool result5 = fakePerson.AccCreated("4892579");

        //AdminCode
        bool result6 = fakePerson.AccCreated("6457823");

        //Assert
        Assert.IsTrue(result1);
        Assert.IsTrue(result2);
        Assert.IsTrue(result3);

        Assert.IsFalse(result4);
        Assert.IsFalse(result5);
        Assert.IsFalse(result6);
    }
}