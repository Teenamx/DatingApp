using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class MessageParams :PaginationParams
    {
        public string UserName { get; set; }

        public string Container { get; set; } = "Unread";

    }
}
