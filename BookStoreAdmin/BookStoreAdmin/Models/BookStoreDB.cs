using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BookStoreAdmin.Models
{
    public partial class BookStoreDB : DbContext
    {
        public BookStoreDB()
            : base("name=BookStoreDB")
        {
        }

        public virtual DbSet<account> accounts { get; set; }
        public virtual DbSet<author> authors { get; set; }
        public virtual DbSet<book> books { get; set; }
        public virtual DbSet<cart> carts { get; set; }
        public virtual DbSet<cart_book> cart_book { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<order_book> order_book { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<book>()
                .HasMany(e => e.cart_book)
                .WithRequired(e => e.book)
                .HasForeignKey(e => e.card_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<book>()
                .HasMany(e => e.order_book)
                .WithRequired(e => e.book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<cart>()
                .HasMany(e => e.cart_book)
                .WithRequired(e => e.cart)
                .HasForeignKey(e => e.card_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<order>()
                .HasMany(e => e.order_book)
                .WithRequired(e => e.order)
                .WillCascadeOnDelete(false);
        }
    }
}
