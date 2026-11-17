using RecipeBox.Data;
using RecipeBox.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RecipeBox.Controllers;

public class RecipesController : Controller
{
    private readonly ApplicationDbContext db;

    public RecipesController(ApplicationDbContext context)
    {
        db = context;
    }

    // Se afișează lista tuturor rețetelor, împreună cu categoria din care fac parte,
    // în ordine alfabetică
    // HttpGet implicit
    public IActionResult Index()
    {
        var recipes = from recipe in db.Recipes.Include(r => r.Category)
                      orderby recipe.Title
                      select recipe;

        ViewBag.Recipes = recipes;

        return View();
    }

    // Se afișează o singură rețetă în funcție de id-ul ei, împreună cu categoria
    // din care face parte și cu toate recenziile asociate (în ordinea postării)
    // HttpGet implicit
    public IActionResult Show(int id)
    {
        Recipe? recipe = db.Recipes
                           .Include(r => r.Category)
                           .Include(r => r.Reviews.OrderBy(rv => rv.Date))
                           .Where(r => r.Id == id)
                           .FirstOrDefault();

        // FirstOrDefault returnează null dacă nu există rețeta cu id-ul dat
        if (recipe is null)
        {
            return NotFound();
        }

        ViewBag.Recipe = recipe;

        ViewBag.Category = recipe.Category;

        return View();
    }

    // Se afișează formularul în care se completează datele unei rețete,
    // împreună cu selectarea categoriei din care face parte
    // HttpGet implicit
    public IActionResult New()
    {
        ViewBag.Categories = db.Categories.OrderBy(c => c.CategoryName).ToList();

        return View();
    }

    // Se adaugă rețeta în baza de date
    [HttpPost]
    public IActionResult New(Recipe recipe)
    {
        try
        {
            db.Recipes.Add(recipe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            return RedirectToAction("New");
        }
    }

    // Se editează o rețetă existentă; categoria se selectează dintr-un dropdown
    // HttpGet implicit
    // Se afișează formularul împreună cu datele rețetei din baza de date
    public IActionResult Edit(int id)
    {
        Recipe? recipe = db.Recipes
                           .Include(r => r.Category)
                           .Where(r => r.Id == id)
                           .FirstOrDefault();

        if (recipe is null)
        {
            return NotFound();
        }

        ViewBag.Recipe = recipe;

        ViewBag.Category = recipe.Category;

        ViewBag.Categories = db.Categories.OrderBy(c => c.CategoryName).ToList();

        return View();
    }

    // Se salvează în baza de date rețeta modificată
    [HttpPost]
    public IActionResult Edit(int id, Recipe requestRecipe)
    {
        Recipe? recipe = db.Recipes.Find(id);

        if (recipe is null)
        {
            return NotFound();
        }

        try
        {
            recipe.Title = requestRecipe.Title;
            recipe.Description = requestRecipe.Description;
            recipe.Ingredients = requestRecipe.Ingredients;
            recipe.Instructions = requestRecipe.Instructions;
            recipe.PrepTimeMinutes = requestRecipe.PrepTimeMinutes;
            recipe.Servings = requestRecipe.Servings;
            recipe.CategoryId = requestRecipe.CategoryId;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        catch (Exception)
        {
            // id-ul se transmite ca valoare de rută: /Recipes/Edit/{id}
            return RedirectToAction("Edit", new { id });
        }
    }

    // Se șterge o rețetă din baza de date (recenziile ei se șterg în cascadă)
    [HttpPost]
    public IActionResult Delete(int id)
    {
        Recipe? recipe = db.Recipes.Find(id);

        if (recipe is null)
        {
            return NotFound();
        }

        db.Recipes.Remove(recipe);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
