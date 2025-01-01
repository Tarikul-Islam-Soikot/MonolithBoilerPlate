using System;
namespace MonolithBoilerPlate.Entity.Entities
{
    public class BusinessPerson: Audit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MsicCode { get; set; }
        public string Tin { get; set; }
        public string Sst { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string BusinessActivityDescription { get; set; }

    }
}
