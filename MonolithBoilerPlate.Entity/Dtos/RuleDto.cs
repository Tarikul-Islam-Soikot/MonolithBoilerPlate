namespace MonolithBoilerPlate.Entity.Dtos
{
    public class RuleDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long KpiTypeId { get; set; }
        public long RoleKpiTypeMapperId { get; set; }
        public decimal Weightage { get; set; }
        public long Year { get; set; }
    }
}
