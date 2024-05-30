public class TourId : View
{
    private static IMuseum museum = Program.Museum;
    public static int WhichTourId()
    {
        while (true)
        {
            museum.WriteLine("Enter tour ID:\n");
            string input = museum.ReadLine();

            if (input.ToLower() == "b" || input.ToLower() == "back")
            {
                return -1;
            }
            else if (int.TryParse(input, out int tourID))
            {
                return tourID;
            }
            else
            {
                museum.WriteLine("Invalid input. Please enter a valid numeric tour ID.");
                museum.WriteLine("Press any key to continue...\n");
                museum.ReadKey();
            }
        }
    }
}
