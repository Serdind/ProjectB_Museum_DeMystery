using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;

public class Person
{
    [JsonPropertyName("QR")]
    public string QR;

    public Person(string qr)
    {
        QR = qr;
    }

    public string Login(string qr)
    {
        List<Visitor> visitors = Tour.LoadVisitorsFromFile();
        List<Guide> guides = Tour.LoadGuidesFromFile();
        List<DepartmentHead> admins = Tour.LoadAdminsFromFile();

        Visitor visitor = visitors.FirstOrDefault(v => v.QR == qr);

        if (visitor != null)
        {
            LoggedIn.VisitorLoginMessageEn(visitor);
            return "Visitor";
        }

        Guide guide = guides.FirstOrDefault(v => v.QR == qr);

        if (guide != null)
        {
            LoggedIn.GuideLoginMessageEn(guide);
            return "Guide";
        }

        DepartmentHead admin = admins.FirstOrDefault(v => v.QR == qr);

        if (admin != null)
        {
            LoggedIn.AdminLoginMessageEn(admin);
            return "Admin";
        }
        return "None";
    }

    public bool AccCreated(string qr)
    {
        List<Guide> guides = Tour.LoadGuidesFromFile();
        List<DepartmentHead> admins = Tour.LoadAdminsFromFile();

        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "unique_codes.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        List<string> uniqueCodes = UniqueCodes.LoadUniqueCodesFromFile(filePath);

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
            if (uniqueCodes.Contains(qr))
            {
                Visitor visitor = new Visitor(0, qr);
                return true;
            }
            else
            {
                Console.WriteLine("Code is not valid.");
                return false;
            }
        }
    }
}