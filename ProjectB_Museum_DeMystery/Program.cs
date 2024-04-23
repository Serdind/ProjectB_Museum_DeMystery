class Program
{
    public static IMuseum Museum = new RealMuseum();
    public static void Main()
    {
        
        Tour.UpdateTours();

        Tour.OverviewTours(false);

        Tour.AddAdminToJSON();

        Tour.AddGuideToJSON();
             
        bool running = true;

        while (running)
        {
            ProgramController.Start();
        }
    }
}