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
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string selectToursDataCommand = @"
                        SELECT * FROM Tours WHERE Id = @TourID";

                    using (var selectData = new SqliteCommand(selectToursDataCommand, connection))
                    {
                        selectData.Parameters.AddWithValue("@TourID", tourID);

                        using (var reader = selectData.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int currentVisitors = reader.GetInt32(reader.GetOrdinal("Visitors"));

                                if (currentVisitors != 13)
                                {
                                    string updateVisitorsCountCommand = @"
                                        UPDATE Tours
                                        SET Visitors = Visitors + 1
                                        WHERE Id = @TourID;";

                                    using (var updateCommand = new SqliteCommand(updateVisitorsCountCommand, connection))
                                    {
                                        updateCommand.Parameters.AddWithValue("@TourID", tourID);
                                        updateCommand.ExecuteNonQuery();
                                    }

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
                                else
                                {
                                    Console.WriteLine("Tour is full");
                                    Tours.ReservateTour(visitor);
                                }
                            }
                        }
                    }
                }

                    return true;
            }
        }
        
        Console.WriteLine("Tour not found. Try again!");
        Tours.ReservateTour(visitor);
        return false;
    }
}