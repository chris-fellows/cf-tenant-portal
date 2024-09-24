using CFTenantPortal.Models;
using AutoMapper;

namespace CFTenantPortal.Web
{
    /// <summary>
    /// AutoMapper profile
    /// </summary>
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<IssueType, IssueTypeVM>();            

            //CreateMap<Issue, IssueVM>().ReverseMap();

            //CreateMap<Property, PropertyVM>().ReverseMap();

            //CreateMap<PropertyGroup, PropertyGroupVM>().ReverseMap();

            //CreateMap<PropertyOwner, PropertyOwnerVM>().ReverseMap();            

            CreateMap<AddressVM, Address>();

            CreateMap<PropertyVM, Property>()
                 .ForMember(p => p.DocumentIds,
                    opt => opt.MapFrom((src, _, _, context) =>
                    {
                        return src.DocumentList.Documents.Select(d => d.Id).ToList();
                    }));

            CreateMap<PropertyGroupVM, PropertyGroup>()
                 .ForMember(pg => pg.DocumentIds,
                    opt => opt.MapFrom((src, _, _, context) =>
                    {
                        return src.DocumentList.Documents.Select(d => d.Id).ToList();
                    }));

            CreateMap<PropertyOwnerVM, PropertyOwner>()
                .ForMember(po => po.DocumentIds,
                    opt => opt.MapFrom((src, _, _, context) =>
                    {
                        return src.DocumentList.Documents.Select(d => d.Id).ToList();
                    }));
        }
    }
}
