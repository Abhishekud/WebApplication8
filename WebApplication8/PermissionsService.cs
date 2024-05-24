using Microsoft.Extensions.Caching.Memory;

namespace WebApplication8
{
    [Flags]
    public enum Permissions
    {
        None = 0,
        Read = 1 << 0,   // Bit 0
        Write = 1 << 1,  // Bit 1
        Delete = 1 << 2, // Bit 2
                         // Add more permissions as needed
    }

    public class PermissionsService
    {
        private readonly IMemoryCache _cache;

        public PermissionsService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void UpdatePermissions(string userId, Permissions permissions)
        {
            _cache.Set($"permissions_{userId}", permissions, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
        }

        public Permissions GetUserPermissions(string userId)
        {
            if (_cache.TryGetValue($"permissions_{userId}", out Permissions permissions))
            {
                return permissions;
            }
            return Permissions.None;
        }
    }

}
