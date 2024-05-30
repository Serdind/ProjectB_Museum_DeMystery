public class LoggedIn : View
{
    private static IMuseum museum = Program.Museum;
    public static void VisitorLoginMessageEn(Visitor visitor)
    {
        museum.WriteLine($"Logged in as: {visitor.QR}");
    }

    public static void GuideLoginMessageEn(Guide guide)
    {
        museum.WriteLine($"Logged in as: {guide.Name}");
    }

    public static void AdminLoginMessageEn(DepartmentHead admin)
    {
        museum.WriteLine($"Logged in as: {admin.Name}");
    }
}