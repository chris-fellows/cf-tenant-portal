﻿using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    public class DocumentBasicVM
    {
        public string Id { get; set; } = String.Empty;

        [MaxLength(100)]
        public string Name { get; set; } = String.Empty;

        public bool AllowDelete { get; set; }
    }
}
