namespace Book_Store.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cart_book
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int card_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int book_id { get; set; }

        public int? quantity { get; set; }

        public int? status { get; set; }

        public int? total_amount { get; set; }

        public virtual book book { get; set; }

        public virtual cart cart { get; set; }
    }
}
