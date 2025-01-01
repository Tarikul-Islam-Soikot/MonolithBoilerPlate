using MonolithBoilerPlate.Entity.Enums;

namespace MonolithBoilerPlate.Entity.Entities
{
    public class User: Audit
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public long CompanyId { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public Company Company { get; set; }
    }
}
