using AutoMapper;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Models.Photos;

namespace RailWiki.Shared.Services
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<Album, AlbumResponseModel>();
            CreateMap<Photo, PhotoResponseModel>();
        }        
    }
}
