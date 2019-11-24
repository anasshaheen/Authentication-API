using Microsoft.AspNetCore.Identity;
using System;

namespace ApiAuth.API.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            CreationDate = DateTime.Now;
        }

        public void Update(string name = null, string email = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                Email = email;
            }
        }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
