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
    public int MaxParticipants { get; set; } = 13;

    public GuidedTour(string name, DateTime date, string language)
    {
        ID = nextID++;
        Name = name;
        Date = date;
        Language = language;
        WaitingList = new List<Visitor>();
        ReservedVisitors = new List<Visitor>();
    }

    public bool PlaceReservation(int tourId, Visitor visitor)
    {
        if (tourId == ID)
        {
            if (ReservedVisitors.Count < MaxParticipants)
            {
                ReservedVisitors.Add(visitor);
                string timeOnly = Date.ToString("HH:mm");
                string dateOnly = Date.ToShortDateString();

                Console.WriteLine($"Reservation made by:");
                Console.WriteLine($"Name: {visitor.Name}");
                Console.WriteLine($"Email: {visitor.Email}");
                Console.WriteLine($"Phonenumber: {visitor.Phonenumber}\n");
                Console.WriteLine("Reservation made for tour:");
                Console.WriteLine($"ID: {ID}");
                Console.WriteLine($"Name: {Name}");
                Console.WriteLine($"Language: {Language}");
                Console.WriteLine($"Date: {dateOnly}");
                Console.WriteLine($"Time: {timeOnly}\n");
                return true;
            }
            else
            {
                WaitingList.Add(visitor);
                Console.WriteLine("Sorry, the tour is fully booked. You have been added to the waiting list.\n");
                return false;
            }
        }
        return false;
    }
}
