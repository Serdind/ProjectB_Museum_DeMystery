using System.Text.Json.Serialization;

public class DepartmentHead : Person
{
    private static int lastId = 1;
    [JsonPropertyName("Id")]
    public int Id;
    [JsonPropertyName("Name")]
    public string Name;
    public DepartmentHead(string name, string qr) : base(qr)
    {
        Id = lastId++;
        Name = name;
    }
}