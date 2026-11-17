# RecipeBox

ASP.NET Core MVC (.NET 10) + Entity Framework Core 10.0.14 + ASP.NET Core Identity.
Entități: Category (1) → (M) Recipe (1) → (M) Review.

## Pornire (Visual Studio, Windows, SQL Server LocalDB)
1. Deschide `RecipeBox.slnx`.
2. Tools → NuGet Package Manager → Package Manager Console (Default project: RecipeBox):
       Add-Migration AddRecipeBoxModels
       Update-Database
   (migrația Identity există deja; prima comandă adaugă tabelele Categories, Recipes, Reviews)
3. Rulează proiectul, înregistrează un cont și apasă „Click here to confirm your account”.
4. Adaugă mai întâi o categorie (/Categories/New), apoi rețete (/Recipes/New).

## macOS / Linux
Înlocuiește `DefaultConnection` din appsettings.json cu stringul pentru containerul SQL Server, apoi:
    dotnet ef migrations add AddRecipeBoxModels
    dotnet ef database update
