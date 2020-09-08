using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationTest.Models
{
    public class Team
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Staff Supervisor { get; set; }
    }
}