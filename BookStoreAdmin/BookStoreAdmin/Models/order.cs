namespace BookStoreAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("order")]
    public partial class order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public order()
        {
            order_book = new HashSet<order_book>();
        }

        [Key]
        [DisplayName("Mã đơn hàng")]
        public int order_id { get; set; }

        [DisplayName("Mã tài khoản")]
        [Required(ErrorMessage = "Mã tài khoản là trường bắt buộc.")]
        public int account_id { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime? created_at { get; set; }

        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Địa chỉ là trường bắt buộc.")]
        public string address { get; set; }

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại là trường bắt buộc.")]
        public string phone_number { get; set; }

        [DisplayName("Phí vận chuyển")]
        [Required(ErrorMessage = "Phí vận chuyển là trường bắt buộc.")]
        public decimal shipping_fee { get; set; }

        [DisplayName("Trạng thái")]
        [Required(ErrorMessage = "Trạng thái là trường bắt buộc.")]
        public string status { get; set; }

        [NotMapped]
        [DisplayName("Tổng hóa đơn")]
        public decimal total_bill
        {
            get
            {
                decimal monney = 0;
                foreach (var item in order_book)
                {
                    monney += (decimal)item.total_amount;
                }
                return monney + shipping_fee;
            }
        }

        public virtual account account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_book> order_book { get; set; }
    }
}
