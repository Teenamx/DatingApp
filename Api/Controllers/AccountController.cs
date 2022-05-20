using Api.Data;
using Api.DTO;
using Api.Entities;
using Api.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
   
    public class AccountController : BaseApiController
    {
        //private readonly DataContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper)
        {
           
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
         {
            if (await UserExists(registerDto.UserName)) return BadRequest("UserName is taken");
            var user = mapper.Map<AppUser>(registerDto);
           // using var hmac = new HMACSHA512();


            user.UserName = registerDto.UserName.ToLower();
            //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            //user.PasswordSalt = hmac.Key;

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            var roleResult = await userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(result.Errors); 
            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                Token =await tokenService.CreateToken(user),
                knownAs=user.KnownAs,
                Gender=user.Gender
            };
            
         }
        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
       [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.Users.Include(p=>p.Photos).SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user == null) return Unauthorized("Invalid username");

            //using var hmac = new HMACSHA512(user.PasswordSalt);
            //var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            //for(int i=0;i < computedHash.Length;i++)
            //{
            //    if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            //}
            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();
            return new UserDto
            {
                UserName=user.UserName,
                Token=await tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                knownAs=user.KnownAs,
                Gender=user.Gender
            };

        }

    }
    
}
