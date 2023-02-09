using ISDomain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISServices.Interface
{
    public interface IUserService
    {
        bool ChangeUserRole(string userId);
        List<LibraryUser> findAll();
        bool IsManager(string userId);
        bool IsLibrarian(string userId);

    }
}
