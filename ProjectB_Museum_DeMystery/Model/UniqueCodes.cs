using Newtonsoft.Json;

public class UniqueCodes
{
    private static Random random = new Random();

    public List<int> GenerateUniqueCodes(int count)
    {
        HashSet<int> codesSet = new HashSet<int>();

        while (codesSet.Count < count)
        {
            int randomNumber = random.Next(100000, 1000000);
            codesSet.Add(randomNumber);
        }
        return new List<int>(codesSet);
    }

    public static void SaveCodesToJson(List<int> codes, string fileName)
    {
        string json = JsonConvert.SerializeObject(codes, Formatting.Indented);
        File.WriteAllText(fileName, json);
    }
}