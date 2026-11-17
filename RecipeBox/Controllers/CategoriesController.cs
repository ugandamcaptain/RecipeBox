using RecipeBox.Data;
using RecipeBox.Models;
using Microsoft.AspNetCore.Mvc;

namespace RecipeBox.Controllers;

public class CategoriesController : Controller
{
    private readonly ApplicationDbContext db;

    public CategoriesController(ApplicationDbContext context)
    {
        db = context;
    }

    public IActionResult Index()
    {
        var categories = from category in db.Categories
                         orderby category.CategoryName
                         select category;

        ViewBag.Categories = categories;
        return View();
    }

    public IActionResult Show(int id)
    {
        Category? category = db.Categories.Find(id);

        if (category is null)
        {
            return NotFound();
        }

        ViewBag.Category = category;
        return View();
    }

    public IActionResult New()
    {
        return View();
    }

    [HttpPost]
    public IActionResult New(Category cat)
    {
        try
        {
            db.Categories.Add(cat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            return View();
        }
    }

    public IActionResult Edit(int id)
    {
        Category? category = db.Categories.Find(id);

        if (category is null)
        {
            return NotFound();
        }

        ViewBag.Category = category;
        return View();
    }

    [HttpPost]
    public IActionResult Edit(int id, Category requestCategory)
    {
        Category? category = db.Categories.Find(id);

        if (category is null)
        {
            return NotFound();
        }

        try
        {
            category.CategoryName = requestCategory.CategoryName;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            ViewBag.Category = category;
            return View();
        }
    }

    // La ștergerea unei categorii se șterg automat și toate rețetele
    // din categoria respectivă (cheia externă CategoryId are ON DELETE CASCADE)
    [HttpPost]
    public IActionResult Delete(int id)
    {
        Category? category = db.Categories.Find(id);

        if (category is null)
        {
            return NotFound();
        }

        db.Categories.Remove(category);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

}
