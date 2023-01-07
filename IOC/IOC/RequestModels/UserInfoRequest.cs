namespace IOC.RequestModels
{
    public class UserInfoRequest
    {
        public UserInfoRequest() { }
        public UserInfoRequest(int? id, string name, string surname, string phNumber)
        {
            IDUser = id;
            Name = name;
            Surname = surname;
            PhoneNumber = phNumber;
        }

        public int? IDUser { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
