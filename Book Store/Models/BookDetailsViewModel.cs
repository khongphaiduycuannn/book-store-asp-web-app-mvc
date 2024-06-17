using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Book_Store.Models
{
    public class BookDetailsViewModel
    {
        public book Book { get; set; }
        public List<book> RelatedBooks { get; set; }
    }
}