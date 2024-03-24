using Microsoft.Data.Sqlite;

class Visitor : Person
{
    string connectionString = "Data Source=MyDatabase.db";
    
    public Visitor(string name, string email, string password, string phonenumber) : base(name, email, password, phonenumber){}

    public bool Reservate(int tourID, Visitor visitor)
    {
        foreach (GuidedTour tour in Tours.guidedTour)
        {
            if (tourID == tour.ID)
            {
                if (tour.ReservedVisitors.Count() < tour.MaxParticipants)
                {
                    tour.ReservedVisitors.Add(visitor);
                    string timeOnly = tour.Date.ToString("HH:mm");
                    string dateOnly = tour.Date.ToShortDateString();

                    Console.WriteLine($"Reservation made by:");
                    Console.WriteLine($"Name: {visitor.Name}");
                    Console.WriteLine($"Email: {visitor.Email}");
                    Console.WriteLine($"Phonenumber: {visitor.Phonenumber}\n");
                    Console.WriteLine("Reservation made for tour:");
                    Console.WriteLine($"ID: {tour.ID}");
                    Console.WriteLine($"Name: {tour.Name}");
                    Console.WriteLine($"Language: {tour.Language}");
                    Console.WriteLine($"Date: {dateOnly}");
                    Console.WriteLine($"Time: {timeOnly}\n");

                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();

                        string insertVisitorDataCommand = @"
                        INSERT OR IGNORE INTO VisitorInTour (Id_Visitor, Id_Tour, Date) VALUES 
                            (@Id_Visitor, @Id_Tour, @Date);";

                        using (var insertData = new SqliteCommand(insertVisitorDataCommand, connection))
                        {
                            insertData.Parameters.AddWithValue("@Id_Visitor", visitor.Id);
                            insertData.Parameters.AddWithValue("@Id_Tour", tour.ID);
                            insertData.Parameters.AddWithValue("@Date", tour.Date);

                            insertData.ExecuteNonQuery();
                        }
                    }

                    return true;
                }
                else
                {
                    tour.WaitingList.Add(visitor);
                    Console.WriteLine("Sorry, the tour is fully booked. You have been added to the waiting list.\n");
                    return false;
                }
            }
        }
        
        Console.WriteLine("Tour not found. Try again!");
        return false;
    }
}