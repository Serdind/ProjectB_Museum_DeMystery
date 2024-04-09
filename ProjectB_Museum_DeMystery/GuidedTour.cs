class GuidedTour
{
    private static long nextID = 1;
    public long ID;
    public string Name;
    public DateTime Date;
    public const string StartingPoint = "Room 1";
    public const string EndPoint = "Room 6";
    public string Language;
    public string NameGuide;
    public List <Visitor> ReservedVisitors;

    public GuidedTour(string name, DateTime date, string language, string nameGuide)
    {
        ID = nextID++;
        Name = name;
        Date = date;
        Language = language;
        NameGuide = nameGuide;
        ReservedVisitors = new List<Visitor>();
    }
}