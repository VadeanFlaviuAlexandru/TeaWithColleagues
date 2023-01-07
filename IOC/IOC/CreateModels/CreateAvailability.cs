using IOC.Constants;

namespace IOC.CreateModels
{
    public class CreateAvailability
    {
        public int IdUser { get; set; }

        public int? IdParticipant { get; set; }

        public string? Location { get; set; }

        public DateTime StartDate { get; set; }
    }
}
