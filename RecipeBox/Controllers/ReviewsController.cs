using RecipeBox.Data;
using RecipeBox.Models;
using Microsoft.AspNetCore.Mvc;

namespace RecipeBox.Controllers;

public class ReviewsController : Controller
{
    private readonly ApplicationDbContext db;

    public ReviewsController(ApplicationDbContext context)
    {
        db = context;
    }

    // Se adaugă o recenzie nouă; formularul se află în Views/Recipes/Show.cshtml
    // Data se completează în Controller, nu se primește din formular
    [HttpPost]
    public IActionResult New(Review review)
    {
        try
        {
            review.Date = DateTime.Now;

            db.Reviews.Add(review);
            db.SaveChanges();

            return RedirectToAction("Show", "Recipes", new { id = review.RecipeId });
        }
        catch (Exception)
        {
            // de exemplu: conținut gol sau rețetă inexistentă
            return RedirectToAction("Show", "Recipes", new { id = review.RecipeId });
        }
    }

    // Se afișează formularul de editare a unei recenzii
    // HttpGet implicit
    public IActionResult Edit(int id)
    {
        Review? review = db.Reviews.Find(id);

        if (review is null)
        {
            return NotFound();
        }

        ViewBag.Review = review;

        return View();
    }

    // Se salvează recenzia modificată
    [HttpPost]
    public IActionResult Edit(int id, Review requestReview)
    {
        Review? review = db.Reviews.Find(id);

        if (review is null)
        {
            return NotFound();
        }

        try
        {
            review.Content = requestReview.Content;

            db.SaveChanges();

            return RedirectToAction("Show", "Recipes", new { id = review.RecipeId });
        }
        catch (Exception)
        {
            return RedirectToAction("Edit", new { id });
        }
    }

    // Se șterge o recenzie și se revine la pagina rețetei
    [HttpPost]
    public IActionResult Delete(int id)
    {
        Review? review = db.Reviews.Find(id);

        if (review is null)
        {
            return NotFound();
        }

        int recipeId = review.RecipeId;

        db.Reviews.Remove(review);
        db.SaveChanges();

        return RedirectToAction("Show", "Recipes", new { id = recipeId });
    }
}
