﻿

namespace MonolithBoilerPlate.Entity.Entities
{
    public class Audit
    {
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
