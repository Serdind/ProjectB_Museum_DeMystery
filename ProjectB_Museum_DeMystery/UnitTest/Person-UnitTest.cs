using NUnit.Framework;

[TestFixture]
public class PersonTests
{
    [Test]
    public void Login_ValidQR_ReturnsRole()
    {
        // Arrange
        var person = new Person("12");

        // Act
        var role = person.Login("12");

        // Assert
        Assert.AreEqual("Visitor", role);
    }

    [Test]
    public void AccCreated_ValidQR_ReturnsTrue()
    {
        // Arrange
        var person = new Person("12");

        // Act
        var result = person.AccCreated("12");

        // Assert
        Assert.IsTrue(result);
    }
}