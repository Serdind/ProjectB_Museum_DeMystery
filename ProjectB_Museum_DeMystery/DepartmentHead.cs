class DepartmentHead : Person
{
    private static int lastId = 1;
    public int Id;
    public string Name;
    public DepartmentHead(string name, string qr) : base(qr)
    {
        Id = lastId++;
        Name = name;
    }
}