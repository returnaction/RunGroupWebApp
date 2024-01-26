using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public int? Pace { get; set; }
        public int? Milage { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public string? ProfileImageUrl { get; set; }

    }
}
