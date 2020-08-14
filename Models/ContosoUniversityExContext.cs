using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace hw2.Models
{
    /// <summary>
    /// 新增屬性用
    /// </summary>
    public partial class ContosoUniversityContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Department>().Property(e => e.DateModified).HasColumnName("DateModified");
            modelBuilder.Entity<Course>().Property(e => e.DateModified).HasColumnName("DateModified");
            modelBuilder.Entity<Person>().Property(e => e.DateModified).HasColumnName("DateModified");

            modelBuilder.Entity<Department>().Property(e => e.IsDeleted).HasColumnName("IsDeleted");
            modelBuilder.Entity<Course>().Property(e => e.IsDeleted).HasColumnName("IsDeleted");
            modelBuilder.Entity<Person>().Property(e => e.IsDeleted).HasColumnName("IsDeleted");

            modelBuilder.Entity<Department>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            modelBuilder.Entity<Person>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            modelBuilder.Entity<Course>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);

        }

        public override int SaveChanges()
        {
            BeforeModify();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            BeforeModify();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void BeforeModify()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["DateModified"] = DateTime.Now;
                        entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues["DateModified"] = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        entry.CurrentValues["DateModified"] = DateTime.Now;
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }
    }
}