﻿using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class PropertyModel
    {
        public string Id { get; set; } = String.Empty;

        [Display(Name = "Address")]
        public string AddressDescription { get; set; } = String.Empty;

        [Display(Name = "Group")]
        public string PropertyGroupName { get; set; } = String.Empty;

        [Display(Name = "Owner")]
        public string PropertyOwnerName { get; set; } = String.Empty;
    }
}
