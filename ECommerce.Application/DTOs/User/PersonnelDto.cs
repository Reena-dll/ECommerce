using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.User
{
    public class PersonnelDto
    {
        public string AdGroup { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PersonnelNumber { get; set; }
        public string UserName { get; set; }
        public string ManagerUserName { get; set; }
        public string ManagerPersonnelNumber { get; set; }
        public string Base { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Seniority { get; set; }
        public string Mail { get; set; }
        public string FireFlag { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Natio { get; set; }
        public DateTime BirthDate { get; set; }
        public string NetlineTLC { get; set; }
        public string NetlineRank { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
