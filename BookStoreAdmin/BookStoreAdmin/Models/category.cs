namespace BookStoreAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("category")]
    public partial class category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public category()
        {
            books = new HashSet<book>();
        }

        [Key]
        public int category_id { get; set; }

        [StringLength(255)]
        [DisplayName("Tên danh mục")]
        [Required(ErrorMessage = "Tên danh mục là trường bắt buộc.")]
        public string name { get; set; }

        [DisplayName("Mô tả")]
        public string description { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime? created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<book> books { get; set; }
    }
}
