using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Speurzoekers.Data.Entities;
using Speurzoekers.Data.Entities.Avatar;
using Speurzoekers.Data.Entities.Collectible;
using Speurzoekers.Data.Entities.Identity;
using Speurzoekers.Data.Entities.Queries;
using Speurzoekers.Data.Entities.User;
using System;

namespace Speurzoekers.Data
{
    public class SpeurzoekersDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<UserInfoEntity> UserInfos { get; set; }
        public DbSet<UserCollectibleEntity> UserCollectibles { get; set; }
        public DbSet<AvatarTypeEntity> AvatarTypes { get; set; }
        public DbSet<AvatarTypeTextEntity> AvatarTypeTexts { get; set; }

        public DbSet<CollectiblePositionEntity> CollectiblePositions { get; set; }

        public DbSet<CollectibleTypeEntity> CollectibleTypes { get; set; }
        public DbSet<CollectibleTypeChoiceEntity> CollectibleTypeChoices { get; set; }
        public DbSet<CollectibleTypeTextEntity> CollectibleTypeTexts { get; set; }

        public DbSet<RegionEntity> Regions { get; set; }

        public DbSet<ConfigurationEntity> Configuration { get; set; }

        public DbSet<ActivityEntity> Activities { get; set; }

        public DbQuery<SummaryIdQuery> GuidSummary { get; set; }
        public DbQuery<SummaryIndexQuery> IndexSummary { get; set; }

        public SpeurzoekersDbContext()
            :base()
        {

        }

        public SpeurzoekersDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CollectiblePositionEntity>()
                .HasOne(c => c.CollectibleType)
                .WithMany();

            builder.Entity<CollectibleTypeEntity>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<AvatarTypeEntity>()
                .HasIndex(c => c.Index)
                .IsUnique();

            builder.Entity<AvatarTypeEntity>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<CollectibleTypeTextEntity>()
                .HasIndex(ct => new { ct.CollectibleTypeId, ct.TextId })
                .IsUnique();

            builder.Entity<UserInfoEntity>()
                .HasOne(c => c.ActiveAvatar)
                .WithMany(a => a.Users)
                .HasForeignKey(x => x.ActiveAvatarIndex)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<RegionEntity>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
