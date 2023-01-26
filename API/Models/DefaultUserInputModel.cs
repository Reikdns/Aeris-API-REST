using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class DefaultUserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        
    }
}