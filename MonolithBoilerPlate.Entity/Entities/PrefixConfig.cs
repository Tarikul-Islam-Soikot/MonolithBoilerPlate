using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Entity.Entities
{

    public class PrefixConfig: Audit
    {
        public long Id { get; set; }
        public string SourceName { get; set; }
        public string Prefix { get; set; }

    }
}
