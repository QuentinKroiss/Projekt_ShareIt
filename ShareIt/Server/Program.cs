using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShareIt.Server.DataObjects;
using ShareIt.Server.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Konfiguration der Services (Dependency Injection)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

// Verwendung von SQLite als Datenbank
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity (Benutzerauthentifizierung) registrieren
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// JWT-Authentifizierung konfigurieren
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtIssuer"],
                ValidAudience = builder.Configuration["JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]!))
            };
        });
var app = builder.Build();

// Konfiguration der HTTP-Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Automatisches Setup bei Start der Anwendung. Keine manuelle Migration der DB notwendig, beim Starten der Anwendung wird diese automatisch migriert. 
// Da zu Beginn keine Daten vorliegen, wurden zu Demozwecken zwei User fest angelegt sowie jeweils 3 Lebensmittel je User. 
// Innerhalb der App, können weiterhin neue Artikel hochgeladen, bearbeitet oder gelöscht werden. 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    context.Database.EnsureCreated(); 

    var users = new[]
    {
        new { Email = "Deko@gmx.de", Password = "Abc123!" },
        new { Email = "Test@gmx.de", Password = "Test123!" }
    };

    foreach (var u in users)
    {
        if (await userManager.FindByEmailAsync(u.Email) == null)
        {
            var user = new IdentityUser { UserName = u.Email, Email = u.Email };
            await userManager.CreateAsync(user, u.Password);
        }
    }

    // Produkte anlegen, wenn noch keine existieren
    if (!context.FoodItems.Any())
    {
        var dekoUser = await userManager.FindByEmailAsync("Deko@gmx.de");
        var testUser = await userManager.FindByEmailAsync("Test@gmx.de");

        var products = new List<FoodItem>
        {
            new FoodItem
            {
                Name = "Äpfel",
                Price = 0,
                Description = "Frisch geerntete Äpfel aus dem Garten.",
                Category = Category.Lebensmittel,
                MHD = DateTime.Today.AddDays(10),
                Street = "Musterstraße 1",
                PostalCode = "12345",
                City = "Musterstadt",
                ImageUrl = "https://images.pexels.com/photos/102104/pexels-photo-102104.jpeg",
                UserId = dekoUser.Id
            },
            new FoodItem
            {
                Name = "Brot",
                Price = 1,
                Description = "Selbst gebackenes Vollkornbrot.",
                Category = Category.Lebensmittel,
                MHD = DateTime.Today.AddDays(4),
                Street = "Bäckerweg 2",
                PostalCode = "45678",
                City = "Brotheim",
                ImageUrl = "https://images.pexels.com/photos/2434/bread-food-healthy-breakfast.jpg",
                UserId = dekoUser.Id
            },
            new FoodItem
            {
                Name = "Kartoffeln",
                Price = 2,
                Description = "Bio-Kartoffeln vom Feld.",
                Category = Category.Lebensmittel,
                MHD = DateTime.Today.AddDays(5),
                Street = "Feldstraße 3",
                PostalCode = "78901",
                City = "Knollenstadt",
                ImageUrl = "https://images.pexels.com/photos/2286776/pexels-photo-2286776.jpeg",
                UserId = dekoUser.Id
            },
            new FoodItem
            {
                Name = "Tomaten",
                Price = 0,
                Description = "Frische Gartentomaten zum Abholen.",
                Category = Category.Lebensmittel,
                MHD = DateTime.Today.AddDays(5),
                Street = "Tomatenallee 4",
                PostalCode = "23456",
                City = "Rotsaft",
                ImageUrl = "https://images.pexels.com/photos/533280/pexels-photo-533280.jpeg",
                UserId = testUser.Id
            },
            new FoodItem
            {
                Name = "Pfirsich",
                Price = 2,
                Description = "Frische Pfirsiche, leider nicht verbraucht",
                Category = Category.Lebensmittel,
                MHD = DateTime.Today.AddDays(14),
                Street = "Baumweg 9",
                PostalCode = "32100",
                City = "Pfirsichstadt",
                ImageUrl = "https://images.pexels.com/photos/461210/pexels-photo-461210.jpeg",
                UserId = testUser.Id
            },
            new FoodItem
            {
                Name = "Karotten",
                Price = 1.2m,
                Description = "Knackige Karotten frisch aus der Erde.",
                Category = Category.Lebensmittel,
                MHD = DateTime.Today.AddDays(7),
                Street = "Gemüsestraße 12",
                PostalCode = "11111",
                City = "Wurzelstadt",
                ImageUrl = "https://images.pexels.com/photos/65174/pexels-photo-65174.jpeg",
                UserId = testUser.Id
            }
        };

        context.FoodItems.AddRange(products);
        await context.SaveChangesAsync();
    }
}



app.Run();
