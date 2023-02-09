using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISDomain.DomainModels
{
    public class AuthorInBook : BaseEntity
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Biography { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }

    }
}
