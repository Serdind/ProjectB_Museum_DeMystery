public class LoggedIn : View
{
    
    public static void VisitorLoginMessageEn(Visitor visitor)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine($"Welcome visitor!");
        museum.WriteLine($"Logged with barcode: {visitor.QR}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void GuideLoginMessageEn(Guide guide)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine($"Welcome guide!");
        museum.WriteLine($"Logged in as: {guide.Name}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }

    public static void AdminLoginMessageEn(DepartmentHead admin)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine($"Welcome admin!");
        museum.WriteLine($"Logged in as: {admin.Name}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        
    }
}