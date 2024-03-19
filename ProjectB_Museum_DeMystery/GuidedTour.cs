class GuidedTour
{
    public string Name;
    public DateTime Date;
    public const string StartingPoint = "Begin";
    public const string EndPoint = "Eind";
    public string Language;
    public List <Visitor> WaitingList;
    public List <Visitor> ReservedVisitors;

    public GuidedTour(string name, DateTime date, string language)
    {
        Name = name;
        Date = date;
        Language = language;
    }
}