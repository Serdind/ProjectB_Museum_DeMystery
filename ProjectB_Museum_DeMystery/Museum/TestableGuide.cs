using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;

public class TestableGuide
{
    public readonly IMuseum Museum;

    public TestableGuide(IMuseum museum)
    {
        Museum = museum;
    }

    public void StartTour(GuidedTour tour)
    {
        if (tour != null)
        {
            string message = $"Tour has been started:" +
                    $"Tour: {tour.Name}\n" +
                    $"Date: {tour.Date.ToShortDateString()}\n" +
                    $"Time: {tour.Date.ToString("HH:mm")}\n" +
                    $"Language: {tour.Language}\n";

            Museum.WriteLine(message);
        }
        else
        {
            Museum.WriteLine("Tour not found.");
        }
    }
}