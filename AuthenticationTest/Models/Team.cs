﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationTest.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        public Staff Supervisor { get; set; }

        public String TeamLeaderEmail { get; set; }
    }
}