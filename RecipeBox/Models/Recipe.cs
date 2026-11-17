using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models;

public class Recipe
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Titlul este obligatoriu")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrierea rețetei este obligatorie")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingredientele sunt obligatorii")]
    public string Ingredients { get; set; } = string.Empty;

    [Required(ErrorMessage = "Modul de preparare este obligatoriu")]
    public string Instructions { get; set; } = string.Empty;

    // timpul de preparare, în minute
    public int PrepTimeMinutes { get; set; }

    // numărul de porții
    public int Servings { get; set; }

    // relație configurată folosind convențiile de nume din EF:
    // cheia externă CategoryId + proprietatea de navigare Category
    // cheia este obligatorie (int), deci ștergerea unei categorii șterge în cascadă și rețetele ei
    [Required(ErrorMessage = "Categoria este obligatorie")]
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    // o rețetă are mai multe recenzii
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
