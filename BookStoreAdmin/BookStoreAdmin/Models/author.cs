namespace BookStoreAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
        [DisplayName("Mã Tác Giả")]
        public int author_id { get; set; }

        [StringLength(255)]
        [DisplayName("Tên Tác Giả")]
        [Required(ErrorMessage = "Tên tác giả là trường bắt buộc.")]
        public string name { get; set; }

        [StringLength(255)]
        [DisplayName("Ảnh tác giả")]
        [Required(ErrorMessage = "Ảnh là trường bắt buộc.")]
        public string image { get; set; }
        [DisplayName("Mô tả")]
        [Required(ErrorMessage = "Mô tả là trường bắt buộc.")]

        public string description { get; set; }

        public int? is_deleted { get; set; }

        public DateTime? created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<book> books { get; set; }
    }
}
