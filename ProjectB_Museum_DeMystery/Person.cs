using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;

class Person
{
    public int Id;
    public string Name;
    string connectionString = "Data Source=MyDatabase.db";

    public Person(string name)
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
        Name = name;
    }

    public void CreateAccount(string name)
    {
        Visitor visitor = new Visitor(name);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string insertVisitorDataCommand = @"
            INSERT OR IGNORE INTO Visitors (Id, Name) VALUES 
                (@Id, @Name);";

            using (var insertData = new SqliteCommand(insertVisitorDataCommand, connection))
            {
                insertData.Parameters.AddWithValue("@Id", visitor.Id);
                insertData.Parameters.AddWithValue("@Name", visitor.Name);

                insertData.ExecuteNonQuery();
            }
        }
    }

    public string Login()
    {
        Console.WriteLine("Insert your full name:");
        string name = Console.ReadLine();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectVisitorDataCommand = @"
                SELECT * FROM Visitors WHERE Name = @Name;";
            
            string selectDepartmentHeadDataCommand = @"
                SELECT * FROM DepartmentHead WHERE Name = @Name;";
            
            string selectGuideDataCommand = @"
                SELECT * FROM Guide WHERE Name = @Name;";

            using (var selectData = new SqliteCommand(selectVisitorDataCommand, connection))
            {
                selectData.Parameters.AddWithValue("@Name", name);

                using (var reader = selectData.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Id = Convert.ToInt32(reader["Id"]);
                        Console.WriteLine($"Type: Visitor\nLogged in as: {reader["Name"]}\n");
                        return "Visitor";
                    }
                }
            }

            using (var selectData2 = new SqliteCommand(selectDepartmentHeadDataCommand, connection))
            {
                selectData2.Parameters.AddWithValue("@Name", name);

                using (var reader2 = selectData2.ExecuteReader())
                {
                    if (reader2.Read())
                    {
                        Id = Convert.ToInt32(reader2["Id"]);
                        Console.WriteLine($"Type: Admin\nLogged in as: {reader2["Name"]}");
                        return "Admin";
                    }
                }
            }

            using (var selectData3 = new SqliteCommand(selectGuideDataCommand, connection))
            {
                selectData3.Parameters.AddWithValue("@Name", name);

                using (var reader3 = selectData3.ExecuteReader())
                {
                    if (reader3.Read())
                    {
                        Id = Convert.ToInt32(reader3["Id"]);
                        Console.WriteLine($"Type: Guide\nLogged in as: {reader3["Name"]}");
                        return "Guide";
                    }
                }
            }
            CreateAccount(name);
        }
        return "None";
    }
}