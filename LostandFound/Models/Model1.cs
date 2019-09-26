namespace LostandFound.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Goods> Goods { get; set; }
        public virtual DbSet<ItemType> ItemType { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserGoods> UserGoods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemType>()
                .HasMany(e => e.Goods)
                .WithRequired(e => e.ItemType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserGoods>()
                .Property(e => e.Type)
                .IsFixedLength();
        }
    }
}
