

namespace MonolithBoilerPlate.Entity.Entities
{
    public class Company: Audit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
    }
}
