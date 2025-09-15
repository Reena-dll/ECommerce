using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.User
{
    public class EmployeServiceParamtersDto
    {
        public string SearchApiBaseUrl { get; set; }
        public string BoomiClientSecret { get; set; }
        public string BoomiClientId { get; set; }
        public string BoomiApiKey { get; set; }
        public string BoomiUrl { get; set; }
    }
}
