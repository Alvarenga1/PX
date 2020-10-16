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
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public String Title { get; set; }
        [Required]
        public String Message { get; set; }
        public Competition Competition { get; set; }
        public Student Student { get; set; }

    }
}
