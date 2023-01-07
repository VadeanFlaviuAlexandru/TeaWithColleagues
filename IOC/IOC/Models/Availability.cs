using IOC.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IOC.Models
{
    public class Availability
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAvailability { get; set; }

        public int IdUser { get; set; }

        public int? IdParticipant { get; set; }

        public string? Location { get; set; }

        public AvailabilityType Type { get; set; }

        public DateTime StartDate { get; set; }
    }
}
