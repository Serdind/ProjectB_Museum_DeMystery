using NUnit.Framework;

[TestFixture]
public class ToursTests
{
    private List<Guide> guides;

    private void ReservateTour(Visitor visitor, int tourId)
    {
        visitor.TourId = tourId;
    }

    [SetUp]
    public void Setup()
    {
        guides = new List<Guide>();
    }

    [Test]
    public void UpdateTours()
    {
        DateTime yesterday = DateTime.Today.AddDays(-1);
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        File.Create(filePath).Close();

        Assert.IsTrue(File.Exists(filePath), "File should exist after updating tours.");

        File.Delete(filePath);
    }
}
