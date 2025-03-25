using Lost_and_Found.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lost_and_Found.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Post()
        {
            var foundItem = _context.Lostitems.ToList();
            var lostItem = _context.Founditems.ToList();
            var combinedData = Tuple.Create(foundItem, lostItem);
            if (combinedData.Item1.Count == 0 && combinedData.Item2.Count == 0)
            {
                ViewBag.NoResultsMessage = "No post found!.";
            }
            return View(combinedData);
  
        }
        public IActionResult Contact()
        {
            return View();
        }
     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // GET: Founditems/Details/5
        public async Task<IActionResult> found_Details(int? id)
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
        // GET: Lostitems/Details/5
        public async Task<IActionResult>Lost_Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostitem = await _context.Lostitems
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (lostitem == null)
            {
                return NotFound();
            }

            return View(lostitem);
        }

        [HttpGet] // Assuming you're using HttpGet for the search page
        public IActionResult Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                ModelState.AddModelError("Search", "Please enter a valid search term.");
                return View();
            }

            var lostItems = _context.Lostitems
                .Where(i => i.ItemName.Contains(search) || i.ItemCategory.Contains(search) || i.Description.Contains(search) || i.LostArea.Contains(search))
                .ToList();

            var foundItems = _context.Founditems
                .Where(i => i.ItemName.Contains(search) || i.ItemCategory.Contains(search) || i.Description.Contains(search) || i.FoundArea.Contains(search))
                .ToList();

            var searchResults = Tuple.Create(lostItems, foundItems);

            if (searchResults.Item1.Count == 0 && searchResults.Item2.Count == 0)
            {
                ViewBag.NoResultsMessage = "No items matching your search were found.";
            }

            return View(searchResults); // Pass the combined search results to the view
        }


    }
}
