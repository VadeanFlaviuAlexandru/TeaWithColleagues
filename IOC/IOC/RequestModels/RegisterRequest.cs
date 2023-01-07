namespace IOC.RequestModels
{
    public class RegisterRequest
    {
        public RegisterRequest(string name, string surname, string mailAddress, string password, string phoneNumber)
        {
            Name = name;
            Surname = surname;
            MailAddress = mailAddress;
            Password = password;
            PhoneNumber = phoneNumber;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
