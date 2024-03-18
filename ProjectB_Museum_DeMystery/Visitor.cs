class Visitor
{
    public long Code;
    public string Name;
    public string Email;
    public long Phonenumber;
    public GuidedTour guidedTour;

    public Visitor(string name, string email, long phonenumber)
    {
        Name = name;
        Email = email;
        Phonenumber = phonenumber;
    }
}