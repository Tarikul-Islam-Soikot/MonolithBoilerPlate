namespace MonolithBoilerPlate.Entity.ViewModels
{
    public class RuleVm
    {
        public long Id { get; set; }
        public long RoleKpiTypeMapperId { get; set; }
        public string RoleName { get; set; }
        public string KpiTypeName { get; set; }
        public decimal Weightage { get; set; }
        public long year { get; set; }
    }
}
