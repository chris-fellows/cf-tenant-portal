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
            
            CreateMap<Document, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Name));

            CreateMap<Employee, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Name));

            CreateMap<Issue, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Reference));

            CreateMap<IssueStatus, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Description));

            CreateMap<IssueType, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Description));

            CreateMap<Message, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Text));

            CreateMap<MessageType, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Description));

            CreateMap<Property, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Address.ToSummary()));

            CreateMap<PropertyGroup, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Name));

            CreateMap<PropertyOwner, EntityReference>()
                .ForMember(er => er.Id, opt => opt.MapFrom((src) => src.Id))
                .ForMember(er => er.Name, opt => opt.MapFrom((src) => src.Name));

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
