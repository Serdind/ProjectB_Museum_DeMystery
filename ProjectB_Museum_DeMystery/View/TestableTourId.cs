public class TestableTourId : View
{
    public readonly IMuseum Museum;

    public TestableTourId(IMuseum museum)
    {
        Museum = museum;
    }

    public int WhichTourId()
    {
        while (true)
        {
            Museum.WriteLine("Enter tour ID:");
            string input = Museum.ReadLine();

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
                Museum.WriteLine("Invalid input. Please enter a valid numeric tour ID.");
            }
        }
    }
}