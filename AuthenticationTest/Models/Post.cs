using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationTest.Models
{
    public class Post
{
        public int Id { get; set; }
        [Required]
        public String Title { get; set; }
        [Required]
        public String Message { get; set; }
        public int CompetitionID { get; set; }
        public Student Student { get; set; }

    }
}
