using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationTest.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public String Message { get; set; }
        public Post Post { get; set; }
        public Student Student { get; set; }
    }
    
}
