using ISDomain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISDomain.Identity
{
    public enum Role
    {
        ROLE_MANAGER,
        ROLE_LIBRARIAN,
        ROLE_GUEST,
    }
    public class LibraryUser : IdentityUser
    {
        public Role Role { get; set; } = Role.ROLE_GUEST;
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
