namespace WebApplication8
{
    [Flags]
    public enum Permissions
    {
        None = 0,
        Read = 1 << 0,   // Bit 0
        Write = 1 << 1,  // Bit 1
        Delete = 1 << 2, // Bit 2
        Execute = 1 << 3,
        Create = 1 << 4,
        Modify = 1 << 5,
        View = 1 << 6,
        Upload = 1 << 7,
        Download = 1 << 8,
        Archive = 1 << 9,
        Restore = 1 << 10,
        Manage = 1 << 11,
        Share = 1 << 12,
        Unshare = 1 << 13,
        Approve = 1 << 14,
        Reject = 1 << 15,
        Assign = 1 << 16,
        Revoke = 1 << 17,
        Audit = 1 << 18
    }
}
