using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Numele categoriei este obligatoriu")]
    public string CategoryName { get; set; } = string.Empty;

    // o categorie conține mai multe rețete (one-to-many)
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
