using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokHomework.Data;
using PustokHomework.Models;
using PustokHomework.ViewModel;
using System.Diagnostics;

namespace PustokHomework.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        public HomeController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                Sliders = _context.Sliders.ToList(),
                Features = _context.Features.ToList(),
                FeaturedBooks = _context.Books.Include(x => x.Author).Include(x => x.Images.Where(bi => bi.PosterStatus != null)).Where(x => x.IsFeatured).Take(25).ToList(),
                NewBooks = _context.Books.Include(x => x.Author).Include(x => x.Images.Where(bi => bi.PosterStatus != null)).Where(x => x.IsNew).Take(25).ToList(),
                DiscountedBooks = _context.Books.Include(x => x.Author).Include(x => x.Images.Where(bi => bi.PosterStatus != null)).Where(x => x.DiscountPercent > 0).OrderByDescending(x => x.DiscountPercent).Take(25).ToList(),
            };
            return View(model);
        }
    }
}
