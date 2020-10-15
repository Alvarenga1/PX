using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthenticationTest.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            string email = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.email = email;
            try
            {
                var student = _context.Student.Include("Team").Single(x => x.Email == email);
                ViewBag.student = student;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            var invites = _context.Invite.FromSqlRaw("SELECT * FROM Invite WHERE InvitedStudentEmail = ({0})", email);
            ViewBag.invites = invites;
            return View(await _context.Team.Include("Supervisor").ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team.Include("Supervisor").
                FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            var members = _context.Student.FromSqlRaw("SELECT * FROM Student WHERE TeamId = ({0})", id);
            ViewBag.members = await members.ToListAsync();
            string email = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.email = email;

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // InvalidStudent GET
        public IActionResult InvalidStudent()
        {
            return View();
        }
        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Team team)
        {
            if (ModelState.IsValid)
            {
                string _email = User.FindFirst(ClaimTypes.Name).Value;
                // check whether the user already exists in database
                var student = await _context.Student.FindAsync(_email);
                team.TeamLeaderEmail = student.Email;
                _context.Add(team);
                student.Team = team;
                _context.Update(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        //JOIN TEAM GET
        public async Task<IActionResult> JoinTeam(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        //STAFF JOIN TEAM POST
        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinTeam(int id)
        {
            //Find team by ID
            var team = await _context.Team.FindAsync(id);
            //Find logged in user's email
            string _email = User.FindFirst(ClaimTypes.Name).Value;
            // get staff's email
            var staff = await _context.Staff.FindAsync(_email);
            //assign staff as supervisor of the team
            team.Supervisor = staff;
            //Save changes to DB
            await _context.SaveChangesAsync();
            //Redirecto to index
            return RedirectToAction(nameof(Details), new { id });
        }

        //LEAVE TEAM GET
        public async Task<IActionResult> LeaveTeam(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        //LEAVE TEAM POST
        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveTeam(int id)
        {
            //Find team by ID
            var team = await _context.Team.FindAsync(id);
            if (team != null)
            {
                _context.Database.ExecuteSqlCommand("UPDATE Team SET SupervisorEmail = NULL WHERE Id = ({0})", id);
            }
            //Save changes to DB
            await _context.SaveChangesAsync();
            //Redirecto to index
            return RedirectToAction(nameof(Details), new { id });
        }

        //LEAVE TEAM POST
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveTeamStudent(int id)
        {
            //Find team by ID
            var team = await _context.Team.FindAsync(id);
            string _email = User.FindFirst(ClaimTypes.Name).Value;
            // get staff's email
            var student = await _context.Student.FindAsync(_email);
            if (student != null)
            {
                _context.Database.ExecuteSqlCommand("UPDATE Student SET TeamId = NULL WHERE Email = ({0})", _email);
            }
            //Save changes to DB
            await _context.SaveChangesAsync();
            //Redirecto to index
            return RedirectToAction(nameof(Details), new { id });
        }
        //INVITE POST
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(int id, string email)
        {
            //Find team by ID
            var team = await _context.Team.FindAsync(id);
            // get student by email
            var student = await _context.Student.FindAsync(email);
            if(student == null)
            {
                return RedirectToAction(nameof(InvalidStudent));
            }

            //If there's already one invite in the DB don't create another
            var count = _context.Invite.Count(x => x.InvitedStudent.Email == email && x.Team == team);
            if (count >= 1)
            {
                return RedirectToAction(nameof(Details), new { id });
            }


            var invite = new Invite();
            invite.Team = team;
            invite.InvitedStudent = student;
            _context.Add(invite);
            //Save changes to DB
            await _context.SaveChangesAsync();
            //Redirecto to index
            return RedirectToAction(nameof(Details), new { id });
        }

        //RESPOND POST
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RespondToInvite(int id, string submit)
        {
            //get invite
            var invite = _context.Invite.Include("InvitedStudent").Include("Team").Single(x => x.Id == id);
            switch (submit) {
            case "Accept":
                    //Find logged in user's email
                    string _email = User.FindFirst(ClaimTypes.Name).Value;
                    // get student by email
                    var student = await _context.Student.FindAsync(_email);
                    //In case a student is the team leader of a team
/*                    if (student.Team.TeamLeaderEmail == student.Email)
                    {
                        var members = _context.Student.FromSqlRaw("SELECT * FROM Student WHERE TeamId = ({0})", student.Team.Id);
                    }*/
                    //Assign student to team
                    student.Team = invite.Team;
                    //Delete invite from DB
                    _context.Remove(invite);
                    //Save changes to DB
                    await _context.SaveChangesAsync();
                    //Redirecto to index
                    return RedirectToAction(nameof(Index));
            case "Decline":
                    //Delete invite from DB
                    _context.Remove(invite);
                    //Save changes to DB
                    await _context.SaveChangesAsync();
                    //Redirecto to index
                    return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            if (User.FindFirst(ClaimTypes.Name).Value != team.TeamLeaderEmail)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("UPDATE Team SET Name = ({1}) WHERE Id = ({0})", id,team.Name);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            if (User.FindFirst(ClaimTypes.Name).Value != team.TeamLeaderEmail)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Team.FindAsync(id);
            if (User.FindFirst(ClaimTypes.Name).Value != team.TeamLeaderEmail)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Database.ExecuteSqlCommand("UPDATE Student SET TeamId = NULL WHERE TeamId = ({0})", id);
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Teams/RemoveFromTeam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromTeam(string studentEmail, int id)
        {
            var student = await _context.Student.FindAsync(studentEmail);
            _context.Database.ExecuteSqlCommand("UPDATE Student SET TeamId = NULL WHERE Email = ({0})", studentEmail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.Id == id);
        }
    }
}
