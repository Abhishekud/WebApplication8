namespace WebApplication8
{
    public class RequiredPermissionAttribute : Attribute
    {
        public string Permission { get; }

        public RequiredPermissionAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
