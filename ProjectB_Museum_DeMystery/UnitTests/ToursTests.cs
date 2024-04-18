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

    [Test]
    public void OverviewTours_EditFalse_CurrentDate()
    {
        bool edit = false;
        DateTime currentDate = DateTime.Today;
        string expectedDate = currentDate.ToShortDateString();
        string expectedTime = currentDate.ToString("HH:mm");

        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            Tours.OverviewTours(edit);
            string result = sw.ToString().Trim();

            Assert.IsTrue(result.Contains(expectedDate), "Expected date not found.");
            Assert.IsTrue(result.Contains(expectedTime), "Expected time not found.");
        }
    }

    [Test]
    public void OverviewTours_EditTrue_TomorrowDate()
    {
        bool edit = true;
        DateTime currentDate = DateTime.Today.AddDays(1);
        string expectedDate = currentDate.ToShortDateString();
        string expectedTime = currentDate.ToString("HH:mm");

        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            Tours.OverviewTours(edit);
            string result = sw.ToString().Trim();

            Assert.IsTrue(result.Contains(expectedDate), "Expected date not found.");
            Assert.IsTrue(result.Contains(expectedTime), "Expected time not found.");
        }
    }
}
