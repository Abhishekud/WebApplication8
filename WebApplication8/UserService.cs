namespace WebApplication8
{
    public class UserService
    {
        private readonly PermissionsService _permissionsService;
        private readonly UserPermissionRepository _userPermissionRepository;

        public UserService(PermissionsService permissionsService, UserPermissionRepository userPermissionRepository)
        {
            _permissionsService = permissionsService;
            _userPermissionRepository = userPermissionRepository;
        }

        public async Task OnUserLoginAsync(string userId)
        {
            var permissions = await FetchUserPermissionsFromDataSourceAsync(userId);
            await _permissionsService.UpdatePermissions(userId, permissions);
        }

        private async Task<List<string>> FetchUserPermissionsFromDataSourceAsync(string userId)
        {
            // Fetch permissions from your data source
            // This is a placeholder example
            return await _userPermissionRepository.GetUserPermissionsAsync(userId);
        }

        public void OnUserLogout(string userId)
        {
            _permissionsService.ClearPermissions(userId);
        }
    }
}
