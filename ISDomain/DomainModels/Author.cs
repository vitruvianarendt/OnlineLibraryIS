using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISDomain.DomainModels
{

    public class Author : BaseEntity
    {
        [Required]
        [DisplayName("Author")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Date of Birth")]
        public DateTime Birthday { get; set; }

        [Required]
        [DisplayName("Biography")]
        public string Biography { get; set; }

        [Required]
        [DisplayName("Country of Origin")]
        public string Country { get; set; }

        [Required]
        [DisplayName("Image")]
        public string Image { get; set; }


        public virtual ICollection<AuthorInBook> AuthorInBooks { get; set; }
    }
}
