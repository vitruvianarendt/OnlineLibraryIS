using ISDomain.Identity;
using ISRepository.Interface;
using ISServices.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISServices.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ChangeUserRole(string userId)
        {
            throw new NotImplementedException();
        }

        public List<LibraryUser> findAll()
        {
            return (List<LibraryUser>)_userRepository.GetAll();
        }

        public bool IsManager(string userId)
        {
            LibraryUser user = _userRepository.Get(userId);
            if (user.Role == Role.ROLE_MANAGER) return true;
            return false;
        }
        public bool IsLibrarian(string userId)
        {
            LibraryUser user = _userRepository.Get(userId);
            if (user.Role == Role.ROLE_LIBRARIAN) return true;
            return false;
        }
    }
}
