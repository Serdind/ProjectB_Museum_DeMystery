public class LoggedIn : View
{
    public static void VisitorLoginMessageEn(Visitor visitor)
    {
        Console.WriteLine($"Logged in as: {visitor.QR}");
    }

    public static void GuideLoginMessageEn(Guide guide)
    {
        Console.WriteLine($"Logged in as: {guide.Name}");
    }

    public static void AdminLoginMessageEn(DepartmentHead admin)
    {
        Console.WriteLine($"Logged in as: {admin.Name}");
    }
}