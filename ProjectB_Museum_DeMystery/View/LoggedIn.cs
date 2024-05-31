public class LoggedIn : View
{
    private static IMuseum museum = Program.Museum;
    public static void VisitorLoginMessageEn(Visitor visitor)
    {
        museum.WriteLine($"Welcome visitor!");
        museum.WriteLine($"Logged in as: {visitor.QR}");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void GuideLoginMessageEn(Guide guide)
    {
        museum.WriteLine($"Welcome guide!");
        museum.WriteLine($"Logged in as: {guide.Name}");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }

    public static void AdminLoginMessageEn(DepartmentHead admin)
    {
        museum.WriteLine($"Welcome admin!");
        museum.WriteLine($"Logged in as: {admin.Name}");
        museum.WriteLine("Press anything to continue...\n");
        museum.ReadKey();
    }
}