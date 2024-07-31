using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System.Net;

namespace CFTenantPortal.Services
{
    public class PropertyGroupService : IPropertyGroupService
    {
        public Task<List<PropertyGroup>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<PropertyGroup> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(pg => pg.Id == id));
        }

        public Task Update(PropertyGroup propertyGroup)
        {
            return Task.CompletedTask;
        }

        private List<PropertyGroup> GetAllInternal()
        {
            var propertyGroups = new List<PropertyGroup>();

            propertyGroups.Add(new PropertyGroup()
            {
                Id = "1",
                Name = "Building 1",
                Description = "Maidenhead, Berkshire",
                DocumentIds = new List<string>() { "1" }
            });

            propertyGroups.Add(new PropertyGroup()
            {
                Id = "2",
                Name = "Building 2",
                Description = "Maidenhead, Berkshire",
                DocumentIds = new List<string>() { "1" }
            });

            propertyGroups.Add(new PropertyGroup()
            {
                Id = "3",
                Name = "Building 3",
                Description = "Maidenhead, Berkshire",
                DocumentIds = new List<string>() { "1" }
            });

            propertyGroups.Add(new PropertyGroup()
            {
                Id = "4",
                Name = "Building 4",
                Description = "Cookham, Berkshire",
                DocumentIds = new List<string>() { "2" }
            });

            propertyGroups.Add(new PropertyGroup()
            {
                Id = "5",
                Name = "Building 5",
                Description = "Cookham, Berkshire",
                DocumentIds = new List<string>() { "2" }
            });

            return propertyGroups;
        }
    }
}
