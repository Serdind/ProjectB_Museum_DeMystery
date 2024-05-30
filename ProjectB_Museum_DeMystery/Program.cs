public static class Program
{
    public static IMuseum Museum = new RealMuseum();
    public static void Main()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "unique_codes.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName1 = "visitors.json";
        string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);

        Tour.UpdateTours();

        Tour.AddAdminToJSON();

        Tour.AddGuideToJSON();

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
