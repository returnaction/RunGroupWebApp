using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels
{
    public class EditUserDashboardViewModel
    {
        public string Id { get; set; }
        public int? Pace { get; set; }
        public int? Milage { get; set; }

        public IFormFile Image { get; set; }
        public string? URL { get; set; }

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        //public ICollection<Club> Clubs { get; set; }
        //public ICollection<Race> Races { get; set; }
    }
}
