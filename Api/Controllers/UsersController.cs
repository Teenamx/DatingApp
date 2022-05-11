﻿using Api.Data;
using Api.DTO;
using Api.Entities;
using Api.Extensions;
using Api.Helpers;
using Api.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
      
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;


        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.photoService = photoService;

        }

        [HttpGet]
       
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            //var users = await userRepository.GetUsersAsync();

            //var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);

            //return Ok(usersToReturn);
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = user.UserName;
            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female" : "male";

           var users = await userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
          
            return Ok(users);
            

        }

      
        [HttpGet("{username}",Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            /*
            var user= await userRepository.GetMemberAsync(username);

            return mapper.Map<MemberDto>(user);*/

            return await userRepository.GetMemberAsync(username); 

          
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {

           
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            mapper.Map(memberUpdateDto, user);
            userRepository.update(user);
            if (await userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if(user.Photos.Count==0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if (await userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser",new {username=user.UserName }, mapper.Map<PhotoDto>(photo));
               //return mapper.Map<PhotoDto>(photo);
            }
                
            return BadRequest("Problem adding photo");

        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain!=null)
            currentMain.IsMain = false;
            photo.IsMain = true;
            if (await userRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("Failed to set main photo");


        }

        [HttpDelete("delete-photo/{photoId}")]

        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null)
                return NotFound();
            if (photo.IsMain)
                return BadRequest("You cannot delete your main photo");
            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);

            }
            user.Photos.Remove(photo);

            if (await userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");

        }
        
            
        
    }
}
