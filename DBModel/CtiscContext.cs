using JXW.Crawl.Utils;
using Microsoft.EntityFrameworkCore;

namespace JXW.Crawl.DBModel
{
    public class CtiscContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConfigUtil.CtiscConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>().ToTable("Topic","Ctisc");
            modelBuilder.Entity<Attachment>().ToTable("Attachment","Ctisc");
        }

        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }

    }
}