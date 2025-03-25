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
    public class FounditemsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FounditemsController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Founditems
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Founditems.Include(f => f.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Founditems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var founditem = await _context.Founditems
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (founditem == null)
            {
                return NotFound();
            }

            return View(founditem);
        }

        // GET: Founditems/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Founditems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,ItemCategory,Description,FoundArea,Date,ImageFile,UserId")] Founditem founditem)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(founditem.ImageFile.FileName);
                string extension = Path.GetExtension(founditem.ImageFile.FileName);
                founditem.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/img/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await founditem.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(founditem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", founditem.UserId);
            return View(founditem);
        }

        // GET: Founditems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var founditem = await _context.Founditems.FindAsync(id);
            if (founditem == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", founditem.UserId);
            return View(founditem);
        }

        // POST: Founditems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,ItemCategory,Description,FoundArea,Date,ImageFile,UserId")] Founditem founditem)
        {
            if (id != founditem.ItemId)
            {
                return NotFound();
            }
         
            if (ModelState.IsValid)
            {
               
                try
                {


                    //delete image from wwwroot/image

                    if (founditem.Image != null)
                    {
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img", founditem.Image);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }


                    //save the data
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(founditem.ImageFile.FileName);
                    string extension = Path.GetExtension(founditem.ImageFile.FileName);
                    founditem.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/img/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await founditem.ImageFile.CopyToAsync(fileStream);
                    }
                    _context.Update(founditem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FounditemExists(founditem.ItemId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", founditem.UserId);
            return View(founditem);
        }

        // GET: Founditems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var founditem = await _context.Founditems
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (founditem == null)
            {
                return NotFound();
            }

            return View(founditem);
        }

        // POST: Founditems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var founditem = await _context.Founditems.FindAsync(id);

            //delete image from wwwroot/image

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img", founditem.Image);
            if (System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);

            _context.Founditems.Remove(founditem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FounditemExists(int id)
        {
            return _context.Founditems.Any(e => e.ItemId == id);
        }
    }
}
