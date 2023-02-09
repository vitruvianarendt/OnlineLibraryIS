using ISDomain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISRepository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<LibraryUser> GetAll();
        LibraryUser Get(string id);
        void Insert(LibraryUser entity);
        void Update(LibraryUser entity);
        void Delete(LibraryUser entity);
    }
}
