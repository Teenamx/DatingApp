using Api.DTO;
using Api.Entities;
using Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Interface
{
    public interface IUserRepository
    {
        void update(AppUser user);

     


        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUsersByIdAsync(int id);

        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

        Task<MemberDto> GetMemberAsync(string username,bool isCurrentUser);

        Task<string> GetUserGender(string username);

        Task<AppUser> GetUserByPhotoId(int photoId);


    }
}
