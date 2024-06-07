public static class Program
{
    public static IMuseum Museum = new RealMuseum();
    public static void Main()
    {
        Tour.AddAdmin(new DepartmentHead("Frans", "6457823"));
        Tour.AddAdminToJSON();

        Tour.AddAdmin(new DepartmentHead("Hans", "3489223"));
        Tour.AddAdminToJSON();

        Tour.AddAdmin(new DepartmentHead("John", "4612379"));
        Tour.AddAdminToJSON();

        Tour.AddGuide(new Guide("Casper", "4892579"));
        Tour.AddGuideToJSON();

        Tour.AddGuide(new Guide("Bas", "9412821"));
        Tour.AddGuideToJSON();

        Tour.AddGuide(new Guide("Rick", "421627"));
        Tour.AddGuideToJSON();

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
