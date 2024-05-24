namespace WebApplication8
{
    public class UserService
    {
        private readonly PermissionsService _permissionsService;

        public UserService(PermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
        }

        public void OnUserLogin(string userId)
        {
            var permissions = FetchUserPermissionsFromDataSource(userId);
            _permissionsService.UpdatePermissions(userId, permissions);
        }

        private Permissions FetchUserPermissionsFromDataSource(string userId)
        {
            // Fetch permissions from your data source
            // This is a placeholder example
            return Permissions.Read | Permissions.Write | Permissions.Delete;
        }
    }
}
