using Api.Data;
using Api.DTO;
using Api.Entities;
using Api.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
      
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
       
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            //var users = await userRepository.GetUsersAsync();

            //var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);

            //return Ok(usersToReturn);

          //  var users = await userRepository.GetMembersAsync();
            return Ok(await userRepository.GetMembersAsync());
            

        }

      
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            /*
            var user= await userRepository.GetMemberAsync(username);

            return mapper.Map<MemberDto>(user);*/

            return await userRepository.GetMemberAsync(username); 

          
        }
    }
}
