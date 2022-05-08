using Microsoft.AspNetCore.Identity;

namespace ProjectVideo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
