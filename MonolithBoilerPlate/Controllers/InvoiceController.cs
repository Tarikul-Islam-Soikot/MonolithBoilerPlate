using MonolithBoilerPlate.Entity.Dtos;
using MonolithBoilerPlate.Repository.Pagination.Model;
using MonolithBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MonolithBoilerPlate.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;


        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [Route("SaveInvoice")]
        public async Task<IActionResult> SaveInvoiceAsync(InvoiceDto param)
        {
            var res = await _invoiceService.AddAsync(param);
            return Ok(res.InvoiceNo);
        }

        [HttpPost]
        [Route("SaveInvoicePdf")]
        public async Task<IActionResult> SaveInvoicePdfAsync([FromBody] long invoiceId)
        {
            var file = await _invoiceService.ProcessInvoicePdfAsync(invoiceId);
            return File(file.Bytes, file.ContentType, file.Name);
        }

        [HttpGet("getPage")]
        public async Task<IActionResult> GetPage([FromQuery] ResourceQueryParameters args)
        {
            return Ok(await _invoiceService.GetPageAsync(args));
        }

        [HttpPost]
        [Route("SyncInvoice")]
        public async Task SyncInvoiceAsync([FromBody] long invoiceId)
        {
             await _invoiceService.SyncInvoice(invoiceId);
        }

        [HttpGet]
        [Route("GetInvoiceStatus/{invoiceNo}")]
        public async Task<IActionResult> GetInvoiceStatus(string invoiceNo)
        {
            return Ok(await _invoiceService.GetInvoiceStatus(invoiceNo));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("DummyApi")]
        public async Task<IActionResult> DummyApi(InvoiceDto param)
        {
            var res = new
            {
                IrbmUniqueNo = "423423432",
                QrCode = "3213123123212"
            };

            return Ok(res);
        }


        #region For Admin only
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("PushInvoiceToConsumer")]
        //public async Task PushToConsumerAsync(long invoiceId)
        //{
        //    await _invoiceService.PushToConsumerAsync(invoiceId);
        //}
        #endregion

    }
}
