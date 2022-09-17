using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationRepository.Models
{
    public class UserData
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string EmailId { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }
       
    }
}
