using Microsoft.AspNetCore.Identity;

namespace server.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Gender { get; set; }
    }
}
