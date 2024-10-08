﻿using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class PropertyOwnerBasicVM
    {
        public string Id { get; set; } = String.Empty;

        [Display(Name = "Name")]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = String.Empty;

        public bool AllowDelete { get; set; }
    }
}
