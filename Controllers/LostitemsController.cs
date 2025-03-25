using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lost_and_Found.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Lost_and_Found.Controllers
{
    public class LostitemsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public LostitemsController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Lostitems
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Lostitems.Include(l => l.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Lostitems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostitem = await _context.Lostitems
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (lostitem == null)
            {
                return NotFound();
            }

            return View(lostitem);
        }

        // GET: Lostitems/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Lostitems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,ItemCategory,Description,LostArea,Date,ImageFile,UserId")] Lostitem lostitem)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(lostitem.ImageFile.FileName);
                string extension = Path.GetExtension(lostitem.ImageFile.FileName);
                lostitem.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/img/", fileName);
                using(var fileStream = new FileStream(path, FileMode.Create))
                {
                    await lostitem.ImageFile.CopyToAsync(fileStream);
                }
                _context.Add(lostitem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", lostitem.UserId);
            return View(lostitem);
        }

        // GET: Lostitems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostitem = await _context.Lostitems.FindAsync(id);
            if (lostitem == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", lostitem.UserId);
            return View(lostitem);
        }

        // POST: Lostitems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,ItemCategory,Description,LostArea,Date,ImageFile,UserId")] Lostitem lostitem)
        {
            if (id != lostitem.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(lostitem.ImageFile.FileName);
                    string extension = Path.GetExtension(lostitem.ImageFile.FileName);
                    lostitem.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/img/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await lostitem.ImageFile.CopyToAsync(fileStream);
                    }
                    _context.Update(lostitem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostitemExists(lostitem.ItemId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", lostitem.UserId);
            return View(lostitem);
        }

        // GET: Lostitems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostitem = await _context.Lostitems
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (lostitem == null)
            {
                return NotFound();
            }

            return View(lostitem);
        }

        // POST: Lostitems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var lostitem = await _context.Lostitems.FindAsync(id);
            //delete image from wwwroot/image

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img", lostitem.Image);
            if (System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);
            _context.Lostitems.Remove(lostitem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LostitemExists(int id)
        {
            return _context.Lostitems.Any(e => e.ItemId == id);
        }
    }
}
