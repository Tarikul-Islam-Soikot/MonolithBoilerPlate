using MonolithBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MonolithBoilerPlate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        
    }
}
