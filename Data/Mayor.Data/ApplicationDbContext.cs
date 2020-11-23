namespace Mayor.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Models;
    using Mayor.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Citizen> Citizens { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Institution> Institutions { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<IssueAttachment> IssueAttachments { get; set; }

        public DbSet<IssueRequest> IssueRequests { get; set; }

        public DbSet<IssueRequestAttachment> IssueRequestAttachments { get; set; }

        public DbSet<IssueReview> IssueReviews { get; set; }

        public DbSet<IssueTag> IssueTags { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            this.IssueAttachmentConfiguration(builder);
            this.IssueRequestAttachmentConfiguration(builder);
            this.IssueReviewConfiguration(builder);
            this.IssueTagConfiguration(builder);
            this.PictureConfiguration(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }

        private void IssueAttachmentConfiguration(ModelBuilder builder)
        {
            builder.Entity<IssueAttachment>()
                .HasKey(x => new { x.IssueId, x.AttachmentId });
        }

        private void IssueRequestAttachmentConfiguration(ModelBuilder builder)
        {
            builder.Entity<IssueRequestAttachment>()
                .HasKey(x => new { x.IssueRequestId, x.AttachmentId });
        }

        private void IssueReviewConfiguration(ModelBuilder builder)
        {
            builder.Entity<IssueReview>()
                .HasOne(c => c.Citizen).WithMany(ir => ir.IssueReviews)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<IssueReview>()
                .HasOne(i => i.Issue).WithMany(ir => ir.IssueReviews)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<IssueReview>()
                .HasKey(x => new { x.CitizenId, x.IssueId });
        }

        private void IssueTagConfiguration(ModelBuilder builder)
        {
            builder.Entity<IssueTag>()
                .HasKey(x => new { x.TagId, x.IssueId });
        }

        private void PictureConfiguration(ModelBuilder builder)
        {
            builder.Entity<Picture>()
                .HasOne(p => p.Issue).WithOne(i => i.TitlePicture)
                .HasForeignKey<Issue>(i => i.TitlePictureId);
        }
    }
}
