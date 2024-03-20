class Visitor
{
    public int Code;
    public string Name;
    public string Email;
    public string Phonenumber;
    public GuidedTour guidedTour;

    public Visitor(string name, string email, string phonenumber)
    {
        Random random = new Random();
        int uniqueCode = random.Next();
        
        Code = uniqueCode;
        Name = name;
        Email = email;
        Phonenumber = phonenumber;
    }
}