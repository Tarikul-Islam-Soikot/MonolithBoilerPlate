using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Infrastructure.Extension;
using MonolithBoilerPlate.Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MonolithBoilerPlate.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IServiceProvider _serviceProvider;
        public override DatabaseFacade Database => base.Database;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IServiceProvider serviceProvider) : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ManageNonEntities();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var entities = modelBuilder.Model.GetEntityTypes();
            entities.ManageDeletingBehaviorRestrictions();
            entities.ManageDecimalPrecision();
        }

        public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry(entity);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.Audit();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            this.Audit();
            return base.SaveChanges();
        }

        #endregion

        #region Audit
        private void Audit()
        {
            var currentUserInfo = _serviceProvider.GetService<ICurrentUserInfo>();
            var currentUserId = currentUserInfo?.UserId ?? 0;

            var now = DateTime.Now;

            foreach (var entry in base
                .ChangeTracker.Entries<Audit>()
                .Where(e => e.State == EntityState.Added
                         || e.State == EntityState.Modified))
            {
                if (entry.State != EntityState.Added)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = entry.Entity.UpdatedBy != null ? entry.Entity.UpdatedBy : currentUserId;
                }
                else
                {
                    entry.Entity.CreatedBy = entry.Entity.CreatedBy != 0 ? entry.Entity.CreatedBy : currentUserId;
                    entry.Entity.CreatedAt = entry.Entity.CreatedAt == DateTime.MinValue ? now : entry.Entity.CreatedAt;
                }
            }
        }

        #endregion

        #region Entities
        public DbSet<User> Users { get; set; }
        public DbSet<InvoiceApiRequestLog> InvoiceApiRequestLogs { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
        public DbSet<DocumentSetting> DocumentSettings { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<GeneralApiRequestLog> GeneralApiRequestLogs { get; set; }
        public DbSet<PrefixConfig> PrefixConfigs { get; set; }

        #endregion

    }
}
