using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISDomain.DomainModels
{
    public enum Genre
    {
        Poem,
        Stories,
        Fantasy,
        Science,
        Fable,
        Philosophy,

    }
    public class Book : BaseEntity
    {
        [Required]
        [DisplayName("Book Name")]
        public string BookName { get; set; }

        [Required]
        [DisplayName("Book Genre")]
        public Genre BookGenre { get; set; }

        [Required]
        [DisplayName("Author")]
        public string BookAuthor { get; set; }

        [Required]
        [DisplayName("Synopsis")]
        public string Synopsis { get; set; }

        [Required]
        [DisplayName("Cover Image")]
        public string CoverImage { get; set; }

        [Required]
        [DisplayName("Book Price")]
        public int BookPrice { get; set; }

        [Required]
        [DisplayName("Publishing Date")]
        public DateTime Date { get; set; }


        public virtual ICollection<BookInShoppingCart> BooksInShoppingCart { get; set; }
        public virtual ICollection<BookInOrder> BooksInOrder { get; set; }
    }
}
