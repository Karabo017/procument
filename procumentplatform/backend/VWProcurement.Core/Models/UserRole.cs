namespace VWProcurement.Core.Models
{
    public enum UserRole
    {
        PublicViewer = 0,    // Any individual viewing tenders without registration
        Supplier = 1,        // Registered SMEs/vendors who bid on tenders
        Buyer = 2,          // Procurement officers creating and managing tenders
        PlatformManager = 3  // Administrators overseeing platform operations
    }
    
    public enum UserStatus
    {
        Pending = 0,         // Awaiting email verification
        Active = 1,          // Active and verified
        Suspended = 2,       // Temporarily suspended
        Deactivated = 3      // Permanently deactivated
    }
}
