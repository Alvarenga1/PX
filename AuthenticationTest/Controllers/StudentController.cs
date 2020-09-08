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

		public async Task<IActionResult> MyDetails()
		{
			// retrieve the logged-in user's email
			// need to add "using System.Security.Claims;"
			string _email = User.FindFirst(ClaimTypes.Name).Value;
			// check whether the user already exists in database
			var student = await _context.Student.FindAsync(_email);

			if (student == null)
			{
				// if the customer's record doesn't exist yet:
				// validation will not be checked when creating an object, but in web form
				student = new Student { Email = _email };
				return View("~/Views/Student/MyDetailsCreate.cshtml", student);
			}
			else
			{
				// if the customer's record already exists
				return View("~/Views/Student/MyDetailsUpdate.cshtml", student);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Student")]
		public async Task<IActionResult> MyDetailsCreate([Bind("Email,Name")] Student student)
		{
			if (ModelState.IsValid)
			{
				_context.Add(student);
				await _context.SaveChangesAsync();

				return View("~/Views/Student/MyDetailsSuccess.cshtml", student);
			}
			return View(student);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Student")]
		public async Task<IActionResult> MyDetailsUpdate([Bind("Email,Name")] Student student)
		{
			if (ModelState.IsValid)
			{
				_context.Update(student);
				await _context.SaveChangesAsync();

				return View("~/Views/Student/MyDetailsSuccess.cshtml", student);
			}
			return View(student);
		}
	}
}
