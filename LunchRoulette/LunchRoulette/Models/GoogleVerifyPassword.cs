using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchRoulette.Models
{
    public class GoogleVerifyPassword
    {
        public string kind { get; set; }
        public string localId { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public string idToken { get; set; }
        public string registered { get; set; }
        public string refreshToken { get; set; }
        public int expiresIn { get; set; }
    }
}
