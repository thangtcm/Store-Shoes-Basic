using Microsoft.AspNetCore.Identity;
using StoreShoe.Core.Repositories;
using StoreShoe.Data;
using static StoreShoe.Data.ApplicationUser;

namespace StoreShoe.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
