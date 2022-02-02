using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace task.app.Models
{
    public class Role:IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}