using Dapper;
using System.Data.SqlClient;
using WebApplication8.Models;

namespace WebApplication8
{
    public class UserPermissionRepository
    {
        private readonly string _connectionString;

        public UserPermissionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<UserPermission> GetUserPermissionAsync(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT UserId, Permissions FROM UserPermissions WHERE UserId = @UserId";
                var result = await connection.QuerySingleOrDefaultAsync<UserPermission>(query, new { UserId = userId });
                return result;
            }
        }

        public async Task UpdateUserPermissionAsync(UserPermission userPermission)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO UserPermissions (UserId, Permissions) VALUES (@UserId, @Permissions) " +
                            "ON DUPLICATE KEY UPDATE Permissions = @Permissions";
                await connection.ExecuteAsync(query, userPermission);
            }
        }
    }

}
