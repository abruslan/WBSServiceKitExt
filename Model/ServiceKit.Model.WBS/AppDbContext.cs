using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceKit.IdentityService;
using ServiceKit.Model.Common;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.Model.WBS
{
    public class AppDbContext : DbContext
    {

        private readonly UserService _userService;

        private static DbContextOptions<AppDbContext> migrationOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(@"Data Source=gls-vsb-sql1ims\SPBASE;Initial Catalog=IMS_ServiceKit;Integrated Security=true;Trusted_Connection=true;");
            //optionsBuilder.UseSqlServer(@"Data Source=gls-vsb-sql1ims\SPBASE;Initial Catalog=IMS_ServiceKit_DEV;Integrated Security=true;Trusted_Connection=true;");

            return optionsBuilder.Options;
        }

        [Obsolete("For migrations only!")]
        public AppDbContext() : base(migrationOptions())
        {
        }


        public AppDbContext (DbContextOptions<AppDbContext> options, UserService userService)
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

        public void RollBack()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEditedEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
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


        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<WBS_Item> WBS_Items { get; set; }
        public DbSet<WBS_SyncSystem> WBS_SyncSystems { get; set; }

        public DbSet<WBS_Request> WBS_Requests { get; set; }
        public DbSet<WBS_RequestItem> WBS_RequestItems { get; set; }
        public DbSet<WBS_RequestProjectItem> WBS_RequestProjectItems { get; set; }

        public DbSet<WBS_Project> WBS_Projects { get; set; }
        public DbSet<WBS_ProjectItem> WBS_ProjectItems { get; set; }
        public DbSet<WBS_ProjectPublication> WBS_ProjectPublications { get; set; }
        public DbSet<WBS_ProjectLog> WBS_ProjectLogs { get; set; }

        public DbSet<WBS_SyncLog> WBS_SyncLogs { get; set; }
        public DbSet<WBS_SyncLogItem> WBS_SyncLogItems { get; set; }

    }
}
