using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crud_delicious.Models;

namespace Crud_delicious.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        ViewBag.AllDishes = _context.Dishes.ToList();
        return View();
    }
    // CREATE VIEW
    [HttpGet]
    [Route("dishes/new")]
    public IActionResult Dishes()
    {
        return View("Dishes");
    }
    //CREATE POST
    [HttpPost]
    [Route("dishes/create")]
    public IActionResult Create(Dish newDish)
    {
        if (ModelState.IsValid)
        {
            _context.Dishes.Add(newDish);
            _context.SaveChanges();
            return RedirectToAction("");
        }
        else
        {
            return View("Dishes");
        }
    }
    // READ ID
    [HttpGet]
    [Route("dishes/showdish/{DishId}")]
    public IActionResult ShowDish(int DishId)
    {
        Dish? OneDish = _context.Dishes.FirstOrDefault(d => d.DishId == DishId);
        return View(OneDish);
    }
    [HttpGet]
    [Route("dishes/editdish/{DishId}")]
    public IActionResult EditDish(int DishId)
    {
        Dish? DishEdit = _context.Dishes.FirstOrDefault(d => d.DishId == DishId);
        return View(DishEdit);
    }
    // UPDATE
    [HttpPost]
    [Route("dishes/updatedish/{DishId}")]
    public IActionResult UpdateDish(Dish newDish, int DishId)
    {
        Dish? OldDish = _context.Dishes.FirstOrDefault(i => i.DishId == DishId);
        // 3. Verify that the new instance passes validations
        if (ModelState.IsValid)
        {
            // 4. Overwrite the old version with the new version
            // Yes, this has to be done one attribute at a time
            OldDish.Name = newDish.Name;
            OldDish.Chef = newDish.Chef;
            OldDish.Tastiness = newDish.Tastiness;
            OldDish.Calories = newDish.Calories;
            OldDish.Description = newDish.Description;
            // You updated it, so update the UpdatedAt field!
            OldDish.UpdatedAt = DateTime.Now;
            // 5. Save your changes
            _context.SaveChanges();
            // 6. Redirect to an appropriate page
            return RedirectToAction("ShowDish", new { DishId });
        }
        else
        {
            // 3.5. If it does not pass validations, show error messages
            // Be sure to pass the form back in so you don't lose your changes
            // It should be the old version so we can keep the ID
            return View("EditDish", OldDish);
        }
    }
    // DELETE
    [HttpPost]
    [Route("dishes/deletedish/{DishId}")]
    public IActionResult DeleteDish(int DishId)
    {
        Dish? DishToDelete = _context.Dishes.SingleOrDefault(i => i.DishId == DishId);
        // Once again, it could be a good idea to verify the monster exists before deleting
        _context.Dishes.Remove(DishToDelete);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
