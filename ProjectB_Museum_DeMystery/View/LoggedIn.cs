public class LoggedIn : View
{
    private static IMuseum museum = Program.Museum;
    public static void VisitorLoginMessageEn(Visitor visitor)
    {
        Console.Clear();
        museum.WriteLine($"Welcome visitor!");
        museum.WriteLine($"Logged with barcode: {visitor.QR}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static void GuideLoginMessageEn(Guide guide)
    {
        Console.Clear();
        museum.WriteLine($"Welcome guide!");
        museum.WriteLine($"Logged in as: {guide.Name}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }

    public static void AdminLoginMessageEn(DepartmentHead admin)
    {
        Console.Clear();
        museum.WriteLine($"Welcome admin!");
        museum.WriteLine($"Logged in as: {admin.Name}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        Console.Clear();
    }
}