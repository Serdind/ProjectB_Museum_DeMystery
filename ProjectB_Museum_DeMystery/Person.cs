using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

class Person
{

    public string QR;
    public int Id;
    string connectionString = "Data Source=MyDatabase.db";

    public Person(string qr)
    {
        QR = qr;
    }

    public string Login(string qr)
    {
        List<Visitor> visitors = Tours.LoadVisitorsFromFile();
        List<Guide> guides = Tours.LoadGuidesFromFile();
        List<DepartmentHead> admins = Tours.LoadAdminsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        if (visitor != null)
        {
            Console.WriteLine($"Logged in as: {visitor.QR}");
            return "Visitor";
        }

        Guide guide = guides.FirstOrDefault(v => v.QR == qr);

        if (guide != null)
        {
            Console.WriteLine($"Logged in as: {guide.Name}");
            return "Guide";
        }

        DepartmentHead admin = admins.FirstOrDefault(v => v.QR == qr);

        if (admin != null)
        {
            Console.WriteLine($"Logged in as: {admin.Name}");
            return "Admin";
        }
        return "None";
    }

    public bool AccCreated(string qr)
    {
        List<Guide> guides = Tours.LoadGuidesFromFile();
        List<DepartmentHead> admins = Tours.LoadAdminsFromFile();

        bool isGuide = guides.Any(g => g.QR == qr);
        bool isAdmin = admins.Any(a => a.QR == qr);

        if (isGuide)
        {
            return false;
        }
        else if (isAdmin)
        {
            return false;
        }
        else
        {
            Visitor visitor = new Visitor(0, qr);
            return true;
        }
    }
}