using Biblioteka.Models;
using Biblioteka.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<LibraryDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
// SEED danych testowych
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<LibraryDbContext>();
    db.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[] { "Administrator", "Employee", "Reader" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminEmail = "admin@library.local";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = "admin",
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "System"
        };

        var createResult = userManager.CreateAsync(admin, "Admin!234").GetAwaiter().GetResult();
        await userManager.CreateAsync(admin, "Admin!234");
        await userManager.AddToRoleAsync(admin, "Administrator");
    }

    if (!db.Categories.Any())
    {
        var cat = new Category { Name = "Informatyka" };
        db.Categories.Add(cat);
        db.Books.Add(new Book
        {
            Title = "Przykładowa książka",
            Author = "Jan Kowalski",
            ISBN = "978-123",
            Category = cat,
            Stock = 3,
            Description = "Opis przykładowej książki",
            TableOfContents = "Spis treści przykładowej książki"
        });
        db.SaveChanges();

    }
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();  // 🔥 To jest kluczowy punkt startowy
