class DepartmentHead : Person
{
    public GuidedTour guidedTour;

    public string Name;

    public DepartmentHead(string name, string qr) : base(qr)
    {
        Name = name;
    }
}