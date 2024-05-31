public static class Program
{
    public static IMuseum Museum = new RealMuseum();
    public static void Main()
    {
        string filePath = Model<UniqueCodes>.GetFileNameUniqueCodes();

        string filePath1 = Model<Visitor>.GetFileNameVisitors();

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
