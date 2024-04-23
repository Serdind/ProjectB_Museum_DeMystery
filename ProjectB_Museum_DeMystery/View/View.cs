public class View
{
    public static void WriteLine(string line)
    {
        Program.Museum.WriteLine(line);
    }

    public static int ReadLineInt()
    {
        return Convert.ToInt32(Program.Museum.ReadLine());
    }

    public static string ReadLineString()
    {
        return Program.Museum.ReadLine();
    }
}