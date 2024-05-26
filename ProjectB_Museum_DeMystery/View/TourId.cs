public class TourId : View
{
    public static int WhichTourId()
    {
        while (true)
        {
            Console.WriteLine("Enter tour ID:\n");
            string input = Console.ReadLine();

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
                Console.WriteLine("Invalid input. Please enter a valid numeric tour ID.");
                Console.WriteLine("Press any key to continue...\n");
                Console.ReadKey(true);
            }
        }
    }
}
