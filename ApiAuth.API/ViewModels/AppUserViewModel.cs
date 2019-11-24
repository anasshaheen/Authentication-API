using System;
using System.Collections.Generic;

namespace ApiAuth.API.ViewModels
{
    public class AppUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}