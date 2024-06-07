public class TourId : View
{
    private static IMuseum museum = Program.Museum;
    public static int WhichTourId()
    {
        while (true)
        {
            museum.WriteLine("Enter tour ID:");
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
                museum.WriteLine("Press anything to continue...");
                museum.ReadKey();        
            }
        }
    }
}
