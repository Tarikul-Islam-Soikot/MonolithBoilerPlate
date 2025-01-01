using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Entity.ViewModels
{
    public class TokenVm
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
