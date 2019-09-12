using IntegrateExternalAPIService1.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrateExternalAPIService1.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        //public List<string> Links { get; set; }
        //public Links UserLinks { get; set; }

    }
}
