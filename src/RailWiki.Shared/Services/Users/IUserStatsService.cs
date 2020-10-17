using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Models.Users;

namespace RailWiki.Shared.Services.Users
{
    public interface IUserStatsService
    {
        Task<UserStatsModel> GetStatsByUserId(int id);        
    }

    public class UserStatsService : IUserStatsService
    {
        private readonly IUserService _userService;
        private readonly IRepository<Photo> _photoRepository;
        private readonly IRepository<PhotoLocomotive> _photoLocomotiveRepository;

        public UserStatsService(IUserService userService,
            IRepository<Photo> photoRepository,
            IRepository<PhotoLocomotive> photoLocomotiveRepository)
        {
            _userService = userService;
            _photoRepository = photoRepository;
            _photoLocomotiveRepository = photoLocomotiveRepository;
        }

        public async Task<UserStatsModel> GetStatsByUserId(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            var model = new UserStatsModel();
            model.UserId = user.Id;
            model.UserName = user.FullName;

            model.PhotoCount = await _photoRepository.TableNoTracking
                .Where(x => x.UserId == id)
                .CountAsync();

            model.LocomotiveCount = await _photoLocomotiveRepository.TableNoTracking
                .Where(x => x.Photo.UserId == id)
                .Select(x => x.LocomotiveId)
                .Distinct()
                .CountAsync();

            // TODO: Add rolling stock stats

            model.LocationCount = await _photoRepository.TableNoTracking
                .Where(x => x.UserId == id)
                .Select(x => x.LocationId)
                .Distinct()
                .CountAsync();

            return model;
        }
    }

}
