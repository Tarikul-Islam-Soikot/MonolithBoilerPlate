using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Entity.Dtos
{
    public class InvoiceLineItemDto
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Classification { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? AmountExcludingDT { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? AmountIncludingDT { get; set; }
    }
}
