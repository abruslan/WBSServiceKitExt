using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceKit.CSIT.CSP.Data.Entry;
using ServiceKit.IdentityService;
using ServiceKit.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly UserService _userService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, UserService userService)
            : base(options)
        {
            _userService = userService;
        }

        public override int SaveChanges()
        {
            UpdateBaseEditedEntity();
            var result = base.SaveChanges();
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateBaseEditedEntity();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        private void UpdateBaseEditedEntity()
        {
            var currentUser = _userService.GetCurrentUserName();

            foreach (EntityEntry<BaseEditedEntity> entry in ChangeTracker.Entries<BaseEditedEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUser;
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.ModifiedBy = currentUser;
                        entry.Entity.Modified = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = currentUser;
                        entry.Entity.Modified = DateTime.Now;
                        break;
                }
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClientService>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ClientService>()
                .Property(p => p.YearPrice)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<RegisterItem>()
                .Property(p => p.RegisterRate)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<RegisterItem>()
                .Property(p => p.ReportRate)
                .HasColumnType("decimal(18,2)");
        }

        public DbSet<Register> Registers { get; set; }
        public DbSet<RegisterItem> RegisterItems { get; set; }
        //public DbSet<Report> Reports { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientService> ClientServices { get; set; }
        public DbSet<ClientAnnex> ClientAnnexes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Signer> Signers { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
