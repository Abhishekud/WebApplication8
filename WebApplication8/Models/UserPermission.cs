namespace WebApplication8.Models
{
    public class UserPermission
    {
        public string? UserId { get; set; }
        public int PermissionsBitwise { get; set; } // Permissions stored as bitwise format
    }
}
