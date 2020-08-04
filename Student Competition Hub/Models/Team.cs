using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student_Competition_Hub.Models
{
    public class Team
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public List<Student> Students { get; set; }
        public Staff Supervisor { get; set; }
    }
}
