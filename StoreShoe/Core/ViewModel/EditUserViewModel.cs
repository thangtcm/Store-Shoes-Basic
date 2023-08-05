using Microsoft.AspNetCore.Mvc.Rendering;
using StoreShoe.Data;

namespace StoreShoe.Core.ViewModel
{
    public class EditUserViewModel
    {
        public ApplicationUser? User { get; set; }

        public IList<SelectListItem>? Roles { get; set; }
    }
}
