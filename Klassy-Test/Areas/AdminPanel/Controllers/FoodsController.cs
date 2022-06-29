using Klassy_Test.DAL;
using Klassy_Test.Helpers;
using Klassy_Test.Models;
using Klassy_Test.ViewModels.Foods;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Klassy_Test.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class FoodsController : Controller
    {
        private AppDbContext _context{ get; set; }
        private IWebHostEnvironment _env { get; }
        public IEnumerable<Food> Foods { get; set; }
        public FoodsController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            Foods = _context.Foods;
        }
        public IActionResult Index()
        {
            return View(_context.Foods.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Food food)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!food.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Photo Size Must Be Less Than 200");
            }
            if (!food.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File Type Must Be Image");
            }
            
            Food newFood = new Food
            {
                Name = food.Name,
                Description = food.Description,
                Price = food.Price,
                
            };
            newFood.Url = await food.Photo.SaveFileAsync(Path.Combine(_env.WebRootPath, "images"));
           await _context.Foods.AddAsync(newFood);
           await _context.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var food = _context.Foods.Find(id);
            if (food==null)
            {
                return NotFound();
            }
            var path = Helper.GetPath(_env.WebRootPath, "images", food.Url);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Foods.Remove(food);
             await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id==null)
            {
                return BadRequest();
            }
            Food food = _context.Foods.FirstOrDefault(f=>f.Id==id);
            if (food==null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, FoodsCreateVM food)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Food foodDb = _context.Foods.FirstOrDefault(f => f.Id == id);
            if (food == null)
            {
                return NotFound();
            }
            if (food.Name.ToLower()==foodDb.Name.ToLower()|| food.Description.ToLower() == foodDb.Description.ToLower()|| food.Price == foodDb.Price)
            {
                return RedirectToAction(nameof(Index));
            }
            bool IsExist = Foods.Any(f => f.Name.ToLower() == food.Name.ToLower() );
            if (IsExist)
            {
                ModelState.AddModelError("Name", $"{food.Name} already exists");
                return View();
            }
            foodDb.Name = food.Name;
            foodDb.Description = food.Description;
            foodDb.Price = food.Price;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
