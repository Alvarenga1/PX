using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationTest.Controllers
{
    public class StudentController : Controller
    {

		private readonly ApplicationDbContext _context;

		public StudentController(ApplicationDbContext context)
		{
			_context = context;
		}

		[Authorize(Roles = "Student")]
        public IActionResult Index()
        {
            return View();
        }
	}
}
