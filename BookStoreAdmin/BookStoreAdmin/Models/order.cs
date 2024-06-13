namespace BookStoreAdmin.Models
{
    using System;
    using System.Collections.Generic;
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
        public int order_id { get; set; }

        public int? account_id { get; set; }

        public DateTime? created_at { get; set; }

        public string address { get; set; }
        public string phone_number { get; set; }
        public decimal shipping_fee { get; set; }
        public string status { get; set; }
        [NotMapped]
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
