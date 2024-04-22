using Microsoft.Data.Sqlite;
using Spectre.Console;
using Newtonsoft.Json;

class Guide : Person
{
    private static int lastId = 0;
    public int Id;
    public string Name;
    public Guide(string name, string qr) : base(qr)
    {
        Id = lastId++;
        Name = name;
    }
    public void ViewTours(string guideName)
    {
        DateTime today = DateTime.Today;
        string fileName = "tours.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, fileName);
        string jsonData = File.ReadAllText(filePath);
        
        List<GuidedTour> tours = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);
        
        List<GuidedTour> guideTours = tours.FindAll(tour => tour.NameGuide == guideName && tour.Date.Date == today);
        
        var table = new Table().LeftAligned();
        AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Top)
            .Start(ctx =>
            {
                table.AddColumn("ID");
                table.AddColumn("Name");
                table.AddColumn("Date");
                table.AddColumn("Time");
                table.AddColumn("StartingPoint");
                table.AddColumn("EndPoint");
                table.AddColumn("Language");
                table.AddColumn("Visitors");
                
                foreach (var tour in guideTours)
                {
                    string timeOnly = tour.Date.ToString("HH:mm");
                    string dateOnly = tour.Date.ToShortDateString();
                    
                    table.AddRow(
                        tour.ID.ToString(),
                        tour.Name,
                        dateOnly,
                        timeOnly,
                        GuidedTour.StartingPoint,
                        GuidedTour.EndPoint,
                        tour.Language,
                        tour.ReservedVisitors.Count.ToString()
                    );
                    
                    ctx.Refresh();
                }
            });
    }
}