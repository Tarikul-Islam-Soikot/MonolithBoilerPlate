using Microsoft.Reporting.NETCore;

namespace MonolithBoilerPlate.Service.Helper
{
    public static class RdlcReportHelper
    {
        public static void AssignReportParams(this LocalReport report, Dictionary<string, string> reportParams)
        {
            foreach (var param in reportParams)
            {
                report.SetParameters(new ReportParameter(param.Key, param.Value));
            }
        }

        public static void AssignReportDataSources(this LocalReport report, Dictionary<string, object> datasources)
        {
            foreach (var datasource in datasources)
            {
                report.DataSources.Add(new ReportDataSource(datasource.Key, datasource.Value));
            }
        }
    }
}
