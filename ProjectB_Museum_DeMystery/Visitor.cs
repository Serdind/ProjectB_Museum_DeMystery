class Visitor
{
    public long Code;
    public string Name;
    public string Email;
    public string Phonenumber;
    public GuidedTour guidedTour;

    public Visitor(string name, string email, string phonenumber)
    {
        Name = name;
        Email = email;
        Phonenumber = phonenumber;
    }
}