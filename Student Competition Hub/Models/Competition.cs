using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student_Competition_Hub.Models
{
    public class Competition
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public List<Team> Teams { get; set; }
        public String Credit { get; set; }
    }
}
