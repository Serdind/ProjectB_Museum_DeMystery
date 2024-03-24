class GuidedTour
{
    private static int nextID = 1;
    public int ID;
    public string Name;
    public DateTime Date;
    public const string StartingPoint = "Begin";
    public const string EndPoint = "Eind";
    public string Language;
    public List <Visitor> WaitingList;
    public List <Visitor> ReservedVisitors;
    public int MaxParticipants = 13;

    public GuidedTour(string name, DateTime date, string language)
    {
        ID = nextID++;
        Name = name;
        Date = date;
        Language = language;
        WaitingList = new List<Visitor>();
        ReservedVisitors = new List<Visitor>();
    }
}
