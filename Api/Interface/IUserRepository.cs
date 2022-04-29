using Api.DTO;
using Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Interface
{
    public interface IUserRepository
    {
        void update(AppUser user);

        Task<bool> SaveAllAsync();


        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUsersByIdAsync(int id);

        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<IEnumerable<MemberDto>> GetMembersAsync();

        Task<MemberDto> GetMemberAsync(string username);
    }
}
