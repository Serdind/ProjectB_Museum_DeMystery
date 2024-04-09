class DepartmentHead : Person
{
    public string Name;

    public DepartmentHead(string name, string qr) : base(qr)
    {
        Name = name;
    }
}