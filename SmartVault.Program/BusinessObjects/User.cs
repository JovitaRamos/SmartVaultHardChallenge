using System;

namespace SmartVault.Program.BusinessObjects
{
    public partial class User : BusinessObjectBase
    {
        public int? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? AccountId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
    public partial class User
    {
        public string? OAuthToken { get; set; }
        public string? OAuthRefreshToken { get; set; }
    }
}