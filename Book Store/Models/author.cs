namespace Book_Store.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("author")]
    public partial class author
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public author()
        {
            books = new HashSet<book>();
        }

        [Key]
        public int author_id { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string image { get; set; }

        public string description { get; set; }

        public int? is_deleted { get; set; }

        public DateTime? created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<book> books { get; set; }
    }
}
