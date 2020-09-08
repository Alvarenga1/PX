using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationTest.Models

{
    public class Competition
    {

        public int Id { get; set; }
        public String Name { get; set; }
        public List<Team> Teams { get; set; }
        public String Credit { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
