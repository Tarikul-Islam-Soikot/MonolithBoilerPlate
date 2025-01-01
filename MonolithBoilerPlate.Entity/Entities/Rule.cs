namespace MonolithBoilerPlate.Entity.Entities
{
    public class Rule: Audit
    {
        public long Id { get; set; }
        public long RoleKpiTypeMapperId { get; set; }
        public decimal Weightage { get; set; }
        public long Year { get; set; }
    }
}
