class Visitor
{
    public int Code;
    public string Name;
    public string Email;
    public int Phonenumber;
    public GuidedTour guidedTour;

    public Visitor(string name, string email, int phonenumber)
    {
        Name = name;
        Email = email;
        Phonenumber = phonenumber;
    }
}