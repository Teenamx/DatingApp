
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTO
{
    public class UserDto
    {
        public string UserName { get; set; }

        public string Token { get; set; }

        public string PhotoUrl { get; set; }

        public string knownAs { get; set; }

        public string Gender { get; set; }

    }
}
