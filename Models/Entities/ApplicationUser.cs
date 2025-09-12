using Microsoft.AspNetCore.Identity;

namespace server.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Gender { get; set; }
        public Boolean IsSeller { get; set; } = false;
        public string? SellingLocation { get; set; }
        public string? SellerName { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
    }
}
