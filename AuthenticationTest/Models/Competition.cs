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
        [Required]
        public String Name { get; set; }

        [Display(Name = "Unit Credit")]
        public String Credit { get; set; }
        [Required]
        public String Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
