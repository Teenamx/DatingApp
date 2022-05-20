using Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Interface
{
   public interface ITokenService
    {
       Task<string> CreateToken(AppUser user);
    }
}
