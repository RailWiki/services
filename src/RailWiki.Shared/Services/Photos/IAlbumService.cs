using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Services.FileStorage;

namespace RailWiki.Shared.Services.Photos
{
    public interface IAlbumService
    {
        Task<GetAlbumModel> GetAlbumByIdAsync(int id);
        Task<IEnumerable<GetAlbumModel>> GetAlbumsAsync(int? userId = null, string title = "");
    }

    public class AlbumService : IAlbumService
    {
        private readonly IRepository<Album> _albumRepository;
        private readonly IFilePathHelper _filePathHelper;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public AlbumService(IRepository<Album> albumRepository,
            IFilePathHelper filePathHelper,
            IFileService fileService,
            IMapper mapper)
        {
            _albumRepository = albumRepository;
            _filePathHelper = filePathHelper;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<GetAlbumModel> GetAlbumByIdAsync(int id)
        {
            var album = await _albumRepository.TableNoTracking
                .Include(x => x.User)
                .Include(x => x.Location)
                .ProjectTo<GetAlbumModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == id);

            ResolveAlbumCoverUrl(album);

            return album;
        }

        public async Task<IEnumerable<GetAlbumModel>> GetAlbumsAsync(int? userId = null, string title = "")
        {
            var albums = await _albumRepository.TableNoTracking
                   .Include(x => x.User)
                   .Where(x => (!userId.HasValue || x.UserId == userId.Value)
                       && (string.IsNullOrEmpty(title) || x.Title.Contains(title)))
                   .OrderBy(x => x.Title)
                   .ProjectTo<GetAlbumModel>(_mapper.ConfigurationProvider)
                   .ToListAsync();

            // TODO: Don't like how cover photo URLs are being constructed
            foreach (var album in albums)
            {
                ResolveAlbumCoverUrl(album);
            }

            return albums;
        }

        private void ResolveAlbumCoverUrl(GetAlbumModel album)
        {
            if (string.IsNullOrEmpty(album.CoverPhotoFileName))
            {
                return;
            }

            // Assume we're using the "small" size for now
            var coverPhotoPath = _filePathHelper.ResolveFilePath(album.Id, album.CoverPhotoFileName, "small");
            album.CoverPhotoUrl = _fileService.ResolveFileUrl(coverPhotoPath);
        }
    }
}
