public class TourId : View
{
    
    public static int WhichTourId()
    {
        IMuseum museum = Program.Museum;
        while (true)
        {
            museum.WriteLine("");
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
                museum.WriteLine("");
                museum.WriteLine("Invalid input. Please enter a valid numeric tour ID.");
                museum.WriteLine("Press anything to continue...");
                museum.ReadKey();
                museum.WriteLine("");
                return 0;    
            }
        }
    }
}
