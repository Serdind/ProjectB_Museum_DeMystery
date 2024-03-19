class Program
{
    public static void Main()
    {
        Console.WriteLine("Tours: \n");
        
        foreach (GuidedTour tour in Tours.guidedTour)
        {
            string timeOnly = tour.Date.ToString("HH:mm");
            string dateOnly = tour.Date.ToShortDateString();

            Console.WriteLine($"Name: {tour.Name}");
            Console.WriteLine($"Language: {tour.Language}");
            Console.WriteLine($"Date: {dateOnly}");
            Console.WriteLine($"Time: {timeOnly} \n");
        }
    }
}