class Program
{
    public static IMuseum Museum = new RealMuseum();
    public static void Main()
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "unique_codes.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        Tour.UpdateTours();

        Tour.OverviewTours(false);

        Tour.AddAdminToJSON();

        Tour.AddGuideToJSON();

        if (UniqueCodes.IsNewDay(filePath))
        {
            List<int> codes = UniqueCodes.GenerateUniqueCodes(50);

            UniqueCodes.SaveCodesToJson(codes, filePath);
        }

        bool running = true;

        while (running)
        {
            ProgramController.Start();
        }
    }
}