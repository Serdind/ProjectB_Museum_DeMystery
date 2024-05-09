using System.Text.Json.Serialization;

public class GuidedTour
{
    private static int nextID = 1;
    [JsonPropertyName("Id")]
    public int ID;
    [JsonPropertyName("Name")]
    public string Name;
    [JsonPropertyName("Date")]
    public DateTime Date;
    public const string StartingPoint = "Room 1";
    public const string EndPoint = "Room 6";
    [JsonPropertyName("Language")]
    public string Language;
    [JsonPropertyName("NameGuide")]
    public string NameGuide;
    [JsonPropertyName("ReservedVisitors")]
    public List <Visitor> ReservedVisitors;
    [JsonPropertyName("Status")]
    public bool Status;

    public GuidedTour(string name, DateTime date, string language, string nameGuide)
    {
        ID = nextID++;
        Name = name;
        Date = date;
        Language = language;
        NameGuide = nameGuide;
        ReservedVisitors = new List<Visitor>();
        Status = true;
    }
}