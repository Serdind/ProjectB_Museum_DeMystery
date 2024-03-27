class GuidedTour
{
    private static int nextID = 1;
    public int ID;
    public string Name;
    public DateTime Date;
    public const string StartingPoint = "Begin";
    public const string EndPoint = "Eind";
    public string Language;
    public string NameGuide;
    public List <Visitor> WaitingList;
    public List <Visitor> ReservedVisitors;

    public GuidedTour(string name, DateTime date, string language, string nameGuide)
    {
        ID = nextID++;
        Name = name;
        Date = date;
        Language = language;
        NameGuide = nameGuide;
        WaitingList = new List<Visitor>();
        ReservedVisitors = new List<Visitor>();
    }
}
