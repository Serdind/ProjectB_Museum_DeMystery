using Microsoft.Data.Sqlite;

class Person
{
    public int Id;
    public string Name;
    public string Email;
    public string Password;
    public string Phonenumber;
    string connectionString = "Data Source=MyDatabase.db";

    public Person(string name, string email, string password, string phonenumber)
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
        Email = email;
        Password = password;
        Phonenumber = phonenumber;
    }

    public void CreateAccount()
    {
        Console.WriteLine("Insert your full name:");
        string name = Console.ReadLine();
        Console.WriteLine("Insert your email:");
        string email = Console.ReadLine();
        Console.WriteLine("Insert your password:");
        string password = Console.ReadLine();
        Console.WriteLine("Insert your phonenumber:");
        string phonenumber = Console.ReadLine();

        Visitor visitor = new Visitor(name, email, password, phonenumber);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string insertVisitorDataCommand = @"
            INSERT OR IGNORE INTO Visitors (Id, Name, Email, Password, Phonenumber) VALUES 
                (@Id, @Name, @Email, @Password, @Phonenumber);";

            using (var insertData = new SqliteCommand(insertVisitorDataCommand, connection))
            {
                insertData.Parameters.AddWithValue("@Id", visitor.Id);
                insertData.Parameters.AddWithValue("@Name", visitor.Name);
                insertData.Parameters.AddWithValue("@Email", visitor.Email);
                insertData.Parameters.AddWithValue("@Password", visitor.Password);
                insertData.Parameters.AddWithValue("@Phonenumber", visitor.Phonenumber);

                insertData.ExecuteNonQuery();
            }
        }
    }

    public string Login()
    {
        Console.WriteLine("Insert your email:");
        string email = Console.ReadLine();
        Console.WriteLine("Insert your password:");
        string password = Console.ReadLine();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectVisitorDataCommand = @"
                SELECT * FROM Visitors WHERE Email = @Email AND Password = @Password;";
            
            string selectDepartmentHeadDataCommand = @"
                SELECT * FROM DepartmentHead WHERE Email = @Email AND Password = @Password;";
            
            string selectGuideDataCommand = @"
                SELECT * FROM Guide WHERE Email = @Email AND Password = @Password;";

            using (var selectData = new SqliteCommand(selectVisitorDataCommand, connection))
            {
                selectData.Parameters.AddWithValue("@Email", email);
                selectData.Parameters.AddWithValue("@Password", password);

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
                selectData2.Parameters.AddWithValue("@Email", email);
                selectData2.Parameters.AddWithValue("@Password", password);

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
                selectData3.Parameters.AddWithValue("@Email", email);
                selectData3.Parameters.AddWithValue("@Password", password);

                using (var reader3 = selectData3.ExecuteReader())
                {
                    if (reader3.Read())
                    {
                        Id = Convert.ToInt32(reader3["Id"]);
                        Console.WriteLine($"Type: Admin\nLogged in as: {reader3["Name"]}");
                        return "Guide";
                    }
                }
            }

            Console.WriteLine("You don't have an account");
        }
        return "None";
    }
}