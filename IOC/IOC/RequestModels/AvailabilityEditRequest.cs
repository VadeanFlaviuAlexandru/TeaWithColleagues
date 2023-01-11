namespace IOC.RequestModels
{
    public class AvailabilityEditRequest
    {
        public int IdAvailability { get; set; }

        public int IdUser { get; set; }

        public int? IdParticipant { get; set; }

        public string? Location { get; set; }

        public DateTime StartDate { get; set; }
    }
}
