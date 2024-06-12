public class LoggedIn : View
{
    
    public static void VisitorLoginMessageEn(Visitor visitor)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine($"Welcome visitor!");
        museum.WriteLine($"Logged in with barcode: {visitor.QR}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void GuideLoginMessageEn(Guide guide)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine($"Welcome guide!");
        museum.WriteLine($"Logged in as: {guide.Name}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }

    public static void AdminLoginMessageEn(DepartmentHead admin)
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine($"Welcome admin!");
        museum.WriteLine($"Logged in as: {admin.Name}");
        museum.WriteLine("Press anything to continue...");
        museum.ReadKey();
        museum.WriteLine("");
        
    }
}