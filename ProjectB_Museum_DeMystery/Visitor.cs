using Microsoft.Data.Sqlite;

class Visitor
{
    public int Id;
    public string Name;
    public string Email;
    public string Password;
    public string Phonenumber;
    public GuidedTour guidedTour;
    string connectionString = "Data Source=MyDatabase.db";

    public Visitor(string name, string email, string password, string phonenumber)
    {
        Random random = new Random();
        int uniqueCode = random.Next();

        Id = uniqueCode;
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
        Console.WriteLine($"Logged in as: {visitor.Name}");
    }

    public void Login()
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

            using (var selectData = new SqliteCommand(selectVisitorDataCommand, connection))
            {
                selectData.Parameters.AddWithValue("@Email", email);
                selectData.Parameters.AddWithValue("@Password", password);

                using (var reader = selectData.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (email == reader["Email"].ToString() && password == reader["Password"].ToString())
                        {
                            Console.WriteLine($"Logged in as: {reader["Name"]}");
                        }
                        else
                        {
                            Console.WriteLine("You dont have an account");
                            CreateAccount();
                        }
                    }
                }
            }
        }
    }
}