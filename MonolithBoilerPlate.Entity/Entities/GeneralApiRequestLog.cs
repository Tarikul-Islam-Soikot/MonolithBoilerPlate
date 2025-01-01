using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Entity.Entities
{

    public class GeneralApiRequestLog: Audit
    {
        public long Id { get; set; }
        public long? InvoiceId { get; set; }
        public long? InvoiceApiRequestLogId { get; set; }
        public string RequestBody { get; set; }
        public string ResponseMessage { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }

    }
}
