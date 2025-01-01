using System.Linq.Expressions;

namespace MonolithBoilerPlate.Repository.Extensions
{
    public static class LambdaBuilder
    {
        public static Expression<Func<TEntity, bool>> BuildLambdaForFindByKey<TEntity>(object id)
        {
            var item = Expression.Parameter(typeof(TEntity), "entity");
            var prop = Expression.Property(item, "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
            return lambda;
        }
    }
}
