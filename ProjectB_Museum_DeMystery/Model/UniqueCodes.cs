using Newtonsoft.Json;

public class UniqueCodes
{
    private static Random random = new Random();

    public List<int> GenerateUniqueCodes(int count)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        string adminsFilePath = Path.Combine(userDirectory, subdirectory, "admins.json");
        string guidesFilePath = Path.Combine(userDirectory, subdirectory, "guides.json");

        HashSet<int> existingCodes = new HashSet<int>();
        if (File.Exists(adminsFilePath))
        {
            string json = File.ReadAllText(adminsFilePath);
            var admins = JsonConvert.DeserializeObject<List<DepartmentHead>>(json);
            foreach (var admin in admins)
            {
                existingCodes.Add(Convert.ToInt32(admin.QR));
            }
        }

        if (File.Exists(guidesFilePath))
        {
            string json1 = File.ReadAllText(guidesFilePath);
            var guides = JsonConvert.DeserializeObject<List<Guide>>(json1);
            foreach (var guide in guides)
            {
                existingCodes.Add(Convert.ToInt32(guide.QR));
            }
        }

        HashSet<int> codesSet = new HashSet<int>();

        while (codesSet.Count < count)
        {
            int randomNumber = random.Next(100000, 1000000);
            if (!existingCodes.Contains(randomNumber))
            {
                codesSet.Add(randomNumber);
            }
        }

        return new List<int>(codesSet);
    }

    public static void SaveCodesToJson(List<int> codes, string fileName)
    {
        string json = JsonConvert.SerializeObject(codes, Formatting.Indented);
        File.WriteAllText(fileName, json);
    }

    public static List<string> LoadUniqueCodesFromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            string json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<List<string>>(json);
        }
        else
        {
            return new List<string>();
        }
    }

     public static bool IsNewDay(string filePath)
    {
        if (File.Exists(filePath))
        {
            DateTime lastModified = File.GetLastWriteTime(filePath);

            return lastModified.Date < DateTime.Today;
        }
        return true;
    }
}
