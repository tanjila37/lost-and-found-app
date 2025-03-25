using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lost_and_Found.Models;
using Microsoft.AspNetCore.Http;

namespace Lost_and_Found.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AppDbContext _context;

        public ClaimsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Claims
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Claims.Include(c => c.Item).Include(c => c.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims
                .Include(c => c.Item)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ClaimId == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // GET: Claims/Create
        public IActionResult Create(int itemId)
        {   
            var userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.ItemId = itemId;
            ViewBag.UserId = userId;

            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimId,UserId,ItemId")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
           var founditem = _context.Founditems.Find(claim.ItemId);
           var user = _context.Users.Find(claim.UserId);
            ViewBag.founditem=founditem;
            ViewBag.user=user;
            return View(claim);
        }

        public IActionResult DisplayData()
        {
            // Retrieve the saved data from your database (e.g., using Entity Framework)
            var claims = _context.Claims.ToList(); // Assuming _context represents your database context

            // Pass the data to the view
            return View(claims);
        }

        // GET: Claims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Founditems, "ItemId", "Description", claim.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", claim.UserId);
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaimId,UserId,ItemId")] Claim claim)
        {
            if (id != claim.ClaimId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(claim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaimExists(claim.ClaimId))
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
            ViewData["ItemId"] = new SelectList(_context.Founditems, "ItemId", "Description", claim.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", claim.UserId);
            return View(claim);
        }

        // GET: Claims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims
                .Include(c => c.Item)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ClaimId == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClaimExists(int id)
        {
            return _context.Claims.Any(e => e.ClaimId == id);
        }

        //ClaimCreate-------------------------------------------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public IActionResult CreateClaim(int userId, int itemId)
        {
            // Find the user and found item based on the provided IDs
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            var foundItem = _context.Founditems.FirstOrDefault(item => item.ItemId == itemId);

            if (user != null && foundItem != null)
            {
                // Create a new claim and associate it with the user and found item
                var claim = new Claim
                {
                    UserId = userId,
                    ItemId = itemId
                };

                // Add the claim to the database context and save changes
                _context.Claims.Add(claim);
                _context.SaveChanges();

                // Optionally, you can handle success or provide feedback to the user
                return RedirectToAction("Index", "Claims");
            }
            else
            {
                // Handle the case where the user or found item doesn't exist
                return RedirectToAction("Error");
            }
        }
        // Claim Details-----------------------------------------------------------------------------------------------------------------
        public IActionResult ClaimDetails(int claimId)
        {
            // Find the claim based on the provided ID
            var claim = _context.Claims.Include(c => c.User).Include(c => c.Item).FirstOrDefault(c => c.ClaimId == claimId);

            if (claim != null)
            {
                return View(claim);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
