﻿namespace BookStoreAdmin.Models
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

        [DisplayName("Tác giả")]
        [Required(ErrorMessage = "Mã tác giả là trường bắt buộc.")]
        public int? author_id { get; set; }

        [DisplayName("Danh mục")]
        [Required(ErrorMessage = "Mã danh mục là trường bắt buộc.")]
        public int? category_id { get; set; }

        [StringLength(255)]
        [DisplayName("Tên sách")]
        [Required(ErrorMessage = "Tên sách là trường bắt buộc.")]
        public string name { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Phải tạo bìa sách")]
        [DisplayName("Bìa sách")]
        public string image { get; set; }

        [DisplayName("Mô tả")]
        [Required(ErrorMessage = "Mô tả là trường bắt buộc.")]
        public string description { get; set; }

        [StringLength(255)]
        [DisplayName("Nhà phát hành")]
        [Required(ErrorMessage = "Nhà phát hành là trường bắt buộc.")]
        public string publish_company { get; set; }

        [DisplayName("Năm phát hành")]
        [Range(1900, 2100, ErrorMessage = "Năm phát hành phải nằm trong khoảng từ 1900 đến 2100.")]
        [Required(ErrorMessage = "Năm phát hành là trường bắt buộc.")]
        public int? publish_year { get; set; }

        [DisplayName("Giá")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá phải là một số dương.")]

        [Required(ErrorMessage = "Giá là trường bắt buộc.")]
        public int? price { get; set; }

        [DisplayName("Đã bán")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng đã bán phải là một số dương.")]
        public int? sold { get; set; }

        [DisplayName("Còn lại")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng còn lại phải là một số dương.")]

        [Required(ErrorMessage = "Số lượng còn lại là trường bắt buộc.")]
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
