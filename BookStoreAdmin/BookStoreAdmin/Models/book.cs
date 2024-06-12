namespace BookStoreAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("book")]
    public partial class book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public book()
        {
            cart_book = new HashSet<cart_book>();
            order_book = new HashSet<order_book>();
        }

        [Key]
        public int book_id { get; set; }

        public int? author_id { get; set; }

        public int? category_id { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string image { get; set; }

        public string description { get; set; }

        [StringLength(255)]
        public string publish_company { get; set; }

        public int? publish_year { get; set; }

        public int? price { get; set; }

        public int? sold { get; set; }

        public int? remain { get; set; }

        public int? is_deleted { get; set; }

        public DateTime? created_at { get; set; }

        public virtual author author { get; set; }

        public virtual category category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart_book> cart_book { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_book> order_book { get; set; }
    }
}
