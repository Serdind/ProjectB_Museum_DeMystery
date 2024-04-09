using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;

class Person
{
    public int Id;
    public string QR;
    string connectionString = "Data Source=MyDatabase.db";

    public Person(string qr)
    {
        int lastId = 0;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT MAX(Id) FROM Visitors";
            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                var result = command.ExecuteScalar();
                if (result != DBNull.Value)
                    lastId = Convert.ToInt32(result);
            }
        }
        
        Id = lastId + 1;
        QR = qr;
    }

    public string Login(string qr)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectVisitorDataCommand = @"
                SELECT * FROM Visitors WHERE QR = @Qr;";
            
            string selectDepartmentHeadDataCommand = @"
                SELECT * FROM DepartmentHead WHERE QR = @Qr;";
            
            string selectGuideDataCommand = @"
                SELECT * FROM Guide WHERE QR = @Qr;";

            using (var selectData = new SqliteCommand(selectVisitorDataCommand, connection))
            {
                selectData.Parameters.AddWithValue("@Qr", qr);

                using (var reader = selectData.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Id = Convert.ToInt32(reader["Id"]);
                        Console.WriteLine($"Type: Visitor\nLogged in as: {reader["QR"]}\n");
                        return "Visitor";
                    }
                }
            }

            using (var selectData2 = new SqliteCommand(selectDepartmentHeadDataCommand, connection))
            {
                selectData2.Parameters.AddWithValue("@Qr", qr);

                using (var reader2 = selectData2.ExecuteReader())
                {
                    if (reader2.Read())
                    {
                        Id = Convert.ToInt32(reader2["Id"]);
                        Console.WriteLine($"Type: Admin\nLogged in as: {reader2["QR"]}");
                        return "Admin";
                    }
                }
            }

            using (var selectData3 = new SqliteCommand(selectGuideDataCommand, connection))
            {
                selectData3.Parameters.AddWithValue("@Qr", qr);

                using (var reader3 = selectData3.ExecuteReader())
                {
                    if (reader3.Read())
                    {
                        Id = Convert.ToInt32(reader3["Id"]);
                        Console.WriteLine($"Type: Guide\nLogged in as: {reader3["QR"]}");
                        return "Guide";
                    }
                }
            }
        }
        return "None";
    }
}