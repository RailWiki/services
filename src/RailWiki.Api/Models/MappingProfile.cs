using AutoMapper;
using RailWiki.Api.Models.Entities.Geography;
using RailWiki.Api.Models.Entities.Photos;
using RailWiki.Api.Models.Entities.Roster;
using RailWiki.Api.Models.Entities.Users;
using RailWiki.Shared.Entities.Geography;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Entities.Roster;
using RailWiki.Shared.Entities.Users;

namespace RailWiki.Api.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Geography
            CreateMap<Country, CountryModel>();
            CreateMap<StateProvince, StateProvinceModel>();
            CreateMap<Location, LocationModel>();

            // Photos
            CreateMap<Album, AlbumModel>();
            CreateMap<Category, CategoryModel>();
            CreateMap<PhotoCategory, PhotoCategoryModel>();
            CreateMap<PhotoLocomotive, PhotoLocomotiveModel>();
            CreateMap<Photo, PhotoModel>();
            CreateMap<PhotoRollingStock, PhotoRollingStockModel>();

            // Roster
            CreateMap<Locomotive, LocomotiveModel>();
            CreateMap<RoadAlternateName, RoadAlternateNameModel>();
            CreateMap<Road, RoadModel>();
            CreateMap<RoadType, RoadTypeModel>();
            CreateMap<RollingStockClass, RollingStockClassModel>();
            CreateMap<RollingStock, RollingStockModel>();
            CreateMap<RollingStockType, RollingStockTypeModel>();
            CreateMap<RosterItem, RosterItemModel>();

            // Users
            CreateMap<User, UserModel>();
        }        
    }
}
