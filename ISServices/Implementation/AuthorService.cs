using ISDomain.DomainModels;
using ISDomain.DTO;
using ISDomain.Identity;
using ISRepository.Interface;
using ISServices.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISServices.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;
        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;

        }

        public void CreateNewAuthor(Author t)
        {
            this._authorRepository.Insert(t);
        }

        public void DeleteAuthor(Guid id)
        {
            var Author = this.GetDetailsForAuthor(id);
            this._authorRepository.Delete(Author);
        }

        public List<Author> GetAllAuthors()
        {
            return this._authorRepository.GetAll().ToList();
        }

        public Author GetDetailsForAuthor(Guid? id)
        { 
            return this._authorRepository.Get(id);
        }

        public void UpdateExistingAuthor(Author t)
        {
            this._authorRepository.Update(t);
        }

    }
}
