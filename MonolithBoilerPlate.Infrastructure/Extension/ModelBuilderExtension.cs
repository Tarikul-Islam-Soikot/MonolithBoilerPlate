using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using MonolithBoilerPlate.Entity.Entities;

namespace MonolithBoilerPlate.Infrastructure.Extension
{
    public static class ModelBuilderExtension
    {
        public static void ManageNonEntities(this ModelBuilder modelBuilder)
        {
            var ignorableEntities = typeof(IgnorableEntity)
                 .Assembly
                 .GetTypes()
                 .Where(x => typeof(IgnorableEntity).IsAssignableFrom(x))
                 .ToList();

            ignorableEntities.ForEach(vm =>
            {
                modelBuilder.Entity(vm).HasNoKey().ToTable(vm.Name, t => t.ExcludeFromMigrations());
            });
        }

        public static void ManageDecimalPrecision(this IEnumerable<IMutableEntityType> entities)
        {
            entities
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)
                         || p.ClrType == typeof(decimal?))
                .ToList()
                .ForEach(p =>
                {
                    if (p.GetPrecision() is null)
                        p.SetPrecision(18);
                    if (p.GetScale() is null)
                        p.SetScale(4);
                });
        }

        public static void ManageDeletingBehaviorRestrictions(this IEnumerable<IMutableEntityType> entities)
        {
            entities
               .SelectMany(e => e.GetForeignKeys())
               .ToList()
               .ForEach(relationship => relationship.DeleteBehavior = DeleteBehavior.Restrict);
        }
    }
}
