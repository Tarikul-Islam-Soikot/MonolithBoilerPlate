namespace MonolithBoilerPlate.Common.Interface
{
    public interface ICacheHelper
    {
        Task<T?> GetAsync<T>(string cacheKey);
        Task AddAsync(string cacheKey, object data, uint? durationInSeconds = null);
        Task UpdateAsync(string cacheKey, object data, uint? durationInSeconds = null);
        Task RemoveAsync(string cacheKey);
        Task RemoveByPatternAsync(string cacheKeyStartsWith);
       
    }
}
