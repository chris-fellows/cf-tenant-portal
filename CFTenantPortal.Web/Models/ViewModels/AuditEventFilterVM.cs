﻿using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class AuditEventFilterVM
    {
        [Display(Name = "From Created")]
        public DateTimeOffset StartCreatedDateTime { get; set; }

        [Display(Name = "To Created")]
        public DateTimeOffset EndCreatedDateTime { get; set; }

        [Display(Name = "Event Type")]
        public string AuditEventTypeId { get; set; } = String.Empty;

        [Display(Name = "Property Group")]
        public string PropertyGroupId { get; set; } = String.Empty;

        [Display(Name = "Property")]
        public string PropertyId { set; get; } = String.Empty;

        [Display(Name = "Property Owner")]
        public string PropertyOwnerId { get; set; } = String.Empty;        

        [ValidateNever]
        public List<EntityReference> AuditEventTypeList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyGroupList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyOwnerList { get; set; } = new List<EntityReference>();        
    }
}
