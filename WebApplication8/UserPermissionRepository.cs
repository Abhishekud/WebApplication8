using Dapper;
using System.Data.SqlClient;

namespace WebApplication8
{
    public class UserPermissionRepository
    {
        private readonly string _connectionString;

        public UserPermissionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT p.PermissionName
                    FROM UserPermissions up
                    INNER JOIN Permissions p ON up.PermissionId = p.PermissionId
                    WHERE up.UserId = @UserId";

                var results = await connection.QueryAsync<string>(query, new { UserId = userId });
                return results.AsList();
            }
        }

        public async Task UpdateUserPermissionsAsync(string userId, List<string> permissions)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var deleteQuery = "DELETE FROM UserPermissions WHERE UserId = @UserId";
                await connection.ExecuteAsync(deleteQuery, new { UserId = userId });

                var insertQuery = @"
                    INSERT INTO UserPermissions (UserId, PermissionId)
                    SELECT @UserId, p.PermissionId
                    FROM Permissions p
                    WHERE p.PermissionName = @PermissionName";

                foreach (var permission in permissions)
                {
                    await connection.ExecuteAsync(insertQuery, new { UserId = userId, PermissionName = permission });
                }
            }
        }
    }
}
