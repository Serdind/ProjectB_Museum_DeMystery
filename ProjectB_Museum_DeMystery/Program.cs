public static class Program
{
    public static IMuseum Museum = new RealMuseum();
    public static void Main()
    {
        var admins = new List<DepartmentHead>();
        Tour.AddAdmin(new DepartmentHead("Frans", "99999"), admins);
        Tour.AddAdminToJSON(admins);

        Tour.AddAdmin(new DepartmentHead("Hans", "3489223"), admins);
        Tour.AddAdminToJSON(admins);

        Tour.AddAdmin(new DepartmentHead("John", "4612379"), admins);
        Tour.AddAdminToJSON(admins);

        var guides = new List<Guide>();
        Tour.AddGuide(new Guide("Casper", "2"), guides);
        Tour.AddGuideToJSON(guides);

        Tour.AddGuide(new Guide("Bas", "9412821"), guides);
        Tour.AddGuideToJSON(guides);

        Tour.AddGuide(new Guide("Rick", "421627"), guides);
        Tour.AddGuideToJSON(guides);

        string filePath = Model<UniqueCodes>.GetFileNameUniqueCodes();

        string filePath1 = Model<Visitor>.GetFileNameVisitors();

        Tour.UpdateTours();

        Tour.CreateEmptyJsonFile(filePath1);

        UniqueCodes uniqueCodes = new UniqueCodes();

        if (UniqueCodes.IsNewDay(filePath))
        {
            List<int> codes = uniqueCodes.GenerateUniqueCodes(50);

            UniqueCodes.SaveCodesToJson(codes, filePath);
        }

        bool running = true;

        while (running)
        {
            ProgramController.Start();
        }
    }
}
