using Microsoft.AspNetCore.Identity;

namespace StoreShoe.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
    public class ApplicationRole : IdentityRole
    {

    }
}
