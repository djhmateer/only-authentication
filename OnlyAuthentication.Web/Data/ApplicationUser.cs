using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyAuthentication.Web.Data
{
    public class ApplicationUser
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        //public bool IsAdmin { get; set; }
        //public string CDRole { get; set; }
    }
}
