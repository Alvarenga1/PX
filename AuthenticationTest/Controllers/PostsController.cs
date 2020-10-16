using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthenticationTest.Data;
using AuthenticationTest.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationTest.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            string email = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.email = email;

            return View(await _context.Post.Include("Student").ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var post = await _context.Post.Include("Student")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            string email = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.email = email;

            //var comments = _context.Comment.FromSqlRaw("SELECT * FROM Comment WHERE PostId = ({0})", post.Id);
            var comments = _context.Comment.Include("Student").Include("Post").Where(x => x.Post.Id == post.Id);
            ViewBag.Comments = comments;

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            var post = new Post();

            var competitions = _context.Competition.FromSqlRaw("SELECT * FROM Competition"); ;
            List<SelectListItem> ls = new List<SelectListItem>();
            foreach (var temp in competitions)
            {
                ls.Add(new SelectListItem() { Value = temp.Id.ToString(), Text = temp.Name });
            }
            ViewBag.Competitions = ls;
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Message,CompetitionID")] Post post)
        {
            if (ModelState.IsValid)
            {
                string _email = User.FindFirst(ClaimTypes.Name).Value;
                // check whether the user already exists in database
                var student = await _context.Student.FindAsync(_email);
                post.Student = student;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { post.Id });
            }
            return View(post);
        }


        //Comment POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(int id, string message)
        {
            if (ModelState.IsValid)
            {
                string _email = User.FindFirst(ClaimTypes.Name).Value;
                // check whether the user already exists in database
                var student = await _context.Student.FindAsync(_email);
                var comment = new Comment();
                comment.Student = student;
                comment.Message = message;
                comment.Post = await _context.Post.FindAsync(id);
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Message")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Database.ExecuteSqlCommand("DELETE FROM Comment WHERE PostId = ({0})", id);
                _context.Post.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
