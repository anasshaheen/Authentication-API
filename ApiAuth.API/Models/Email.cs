using System;

namespace ApiAuth.API.Models
{
    public class Email
    {
        public Email()
        {
            CreationDate = DateTime.Now;
        }

        public int Id { get; set; }

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}