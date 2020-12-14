using AutoMapper;
using RailWiki.Shared.Entities.Geography;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Entities.Roster;
using RailWiki.Shared.Entities.Users;
using RailWiki.Shared.Models.Geography;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Models.Roster;
using RailWiki.Shared.Models.Users;

namespace RailWiki.Shared.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Geography
            CreateMap<Country, CountryModel>();
            CreateMap<StateProvince, StateProvinceModel>();
            CreateMap<Location, LocationModel>();
            CreateMap<Location, GetLocationModel>();

            // Photos
            CreateMap<Album, GetAlbumModel>();
            CreateMap<Category, CategoryModel>();
            CreateMap<PhotoCategory, PhotoCategoryModel>();
            CreateMap<PhotoLocomotive, PhotoLocomotiveModel>();
            CreateMap<Photo, GetPhotoModel>();
            CreateMap<PhotoRollingStock, PhotoRollingStockModel>();

            // Roster
            CreateMap<Locomotive, LocomotiveModel>();
            CreateMap<LocomotiveType, LocomotiveTypeModel>();
            CreateMap<RoadAlternateName, RoadAlternateNameModel>();
            CreateMap<Road, RoadModel>();
            CreateMap<RoadType, RoadTypeModel>();
            CreateMap<RollingStockClass, RollingStockClassModel>();
            CreateMap<RollingStock, RollingStockModel>();
            CreateMap<RollingStockType, RollingStockTypeModel>();
            CreateMap<RosterItem, RosterItemModel>();
            CreateMap<Comment, GetCommentModel>();

            // Users
            CreateMap<User, GetUserModel>();
        }        
    }
}
