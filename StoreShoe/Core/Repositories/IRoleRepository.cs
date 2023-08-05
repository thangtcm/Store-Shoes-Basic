using Microsoft.AspNetCore.Identity;

namespace StoreShoe.Core.Repositories
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
