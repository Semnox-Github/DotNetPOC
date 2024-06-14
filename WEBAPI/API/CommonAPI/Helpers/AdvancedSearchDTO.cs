using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.CommonAPI.Games.Helpers
{
    public class AdvancedSearchDTO
    {
        public string Criteria { get; set; }

        public string Operator { get; set; }

        public string Column { get; set; }

        public string Parameter { get; set; }

    }
}