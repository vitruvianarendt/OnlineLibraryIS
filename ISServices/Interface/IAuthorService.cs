using ISDomain.DomainModels;
using ISDomain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISServices.Interface
{
    public interface IAuthorService
    {
        List<Author> GetAllAuthors();
        Author GetDetailsForAuthor(Guid? id);
        void CreateNewAuthor(Author t);
        void UpdateExistingAuthor(Author t);
        void DeleteAuthor(Guid id);

    }
}
