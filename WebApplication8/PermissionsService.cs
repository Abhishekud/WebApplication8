using Microsoft.Extensions.Caching.Memory;

namespace WebApplication8
{
    public class PermissionsService
    {
        private readonly IMemoryCache _cache;
        private readonly UserPermissionRepository _repository;

        public PermissionsService(IMemoryCache cache, UserPermissionRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            if (_cache.TryGetValue($"permissions_{userId}", out List<string> cachedPermissions))
            {
                return cachedPermissions;
            }

            var userPermissionsList = await _repository.GetUserPermissionsAsync(userId);

            _cache.Set($"permissions_{userId}", userPermissionsList, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return userPermissionsList;
        }

        public async Task UpdatePermissions(string userId, List<string> permissions)
        {
            _cache.Set($"permissions_{userId}", permissions, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            await _repository.UpdateUserPermissionsAsync(userId, permissions);
        }

        public void ClearPermissions(string userId)
        {
            _cache.Remove($"permissions_{userId}");
        }
    }
}
