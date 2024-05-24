using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class TestablePerson
{
    public readonly IMuseum Museum;

    public TestablePerson(IMuseum museum)
    {
        Museum = museum;
    }

    public string Login(string qr)
    {
        List<Visitor> visitors = TestJsonData.LoadVisitorsFromTestFile(Museum);
        List<Guide> guides = TestJsonData.LoadGuidesFromFile(Museum);
        List<DepartmentHead> admins = TestJsonData.LoadAdminsFromFile(Museum);

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
        List<Guide> guides = TestJsonData.LoadGuidesFromFile(Museum);
        List<DepartmentHead> admins = TestJsonData.LoadAdminsFromFile(Museum);

        List<string> uniqueCodes = TestJsonData.LoadUniqueCodesFromTestFile(Museum);

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