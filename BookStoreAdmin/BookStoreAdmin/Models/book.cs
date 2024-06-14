namespace BookStoreAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
        [DisplayName("Mã sách")]
        public int book_id { get; set; }

        [DisplayName("Mã tác giả")]
        public int? author_id { get; set; }
        [DisplayName("Mã danh mục")]

        public int? category_id { get; set; }

        [StringLength(255)]
        [DisplayName("Tên sách")]
        public string name { get; set; }

        [StringLength(255)]
        [DisplayName("Bìa sách")]
        public string image { get; set; }
        [DisplayName("Mô tả")]

        public string description { get; set; }

        [StringLength(255)]
        [DisplayName("Nhà phát hành")]
        public string publish_company { get; set; }
        [DisplayName("Năm phát hành")]

        public int? publish_year { get; set; }
        [DisplayName("Giá")]

        public int? price { get; set; }
        [DisplayName("Đã bán")]

        public int? sold { get; set; }
        [DisplayName("Còn lại")]

        public int? remain { get; set; }
        [DisplayName("Trạng thái")]

        public int? is_deleted { get; set; }
        [DisplayName("Ngày tạo")]


        public DateTime? created_at { get; set; }

        public virtual author author { get; set; }

        public virtual category category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart_book> cart_book { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_book> order_book { get; set; }
    }
}
