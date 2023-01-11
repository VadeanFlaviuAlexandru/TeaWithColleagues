namespace IOC.RequestModels
{
    public class AvailabilityDtoGetter
    {
        public AvailabilityDtoGetter(int idUser, int idParticipant, string location, DateTime startDate)
        {
            IDUser = idUser;
            IDPart = idParticipant;
            locationn = location;
            datee = startDate;
        }

        public int IDUser { get; }
        public int IDPart { get; }
        public string locationn { get; }
        public DateTime datee { get; }
    }
}
