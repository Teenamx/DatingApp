using Api.DTO;
using Api.Entities;
using Api.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext context;

        public PhotoRepository(DataContext context)
        {
            this.context = context;
        }
        public async Task<Photo> GetPhotoById(int id)
        {
            return await context.photos.IgnoreQueryFilters().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
        {
            return await context.photos.IgnoreQueryFilters().Where(p => p.IsApproved == false)
                .Select(u => new PhotoForApprovalDto
                {
                    Id = u.Id,
                    Username = u.AppUser.UserName,
                    Url = u.Url,
                    IsApproved = u.IsApproved
                }).ToListAsync();
        }

        public void RemovePhoto(Photo photo)
        {
            context.photos.Remove(photo);
        }
    }
}
