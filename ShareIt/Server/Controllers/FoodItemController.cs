using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareIt.Server.Data;
using ShareIt.Server.DataObjects;
using ShareIt.Shared;

namespace ShareIt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FoodItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFoodItem([FromBody] FoodItemModel foodItemModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Prüfe, ob der Benutzer existiert
            var user = await _userManager.FindByNameAsync(foodItemModel.UserName);
            if (user == null)
            {
                return BadRequest("Benutzer nicht gefunden.");
            }

            var category = (DataObjects.Category)foodItemModel.Category; // Konvertierung der Kategorie aus dem Model

            var foodItem = new FoodItem
            {
                Name = foodItemModel.Name,
                Price = foodItemModel.Price,
                Description = foodItemModel.Description,
                UserId = user.Id,
                ImageUrl = foodItemModel.ImageUrl,
                Category = category,
                MHD = foodItemModel.MHD,
                Street = foodItemModel.Street, 
                PostalCode = foodItemModel.PostalCode,
                City = foodItemModel.City
            };

            try
            {
                _context.FoodItems.Add(foodItem);
                await _context.SaveChangesAsync();
                return Ok(foodItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new FoodItemResult { Successful = false, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodItem(int id)
        {
            var foodItem = await _context.FoodItems
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (foodItem == null)
            {
                return NotFound();
            }

            // Prüfen, ob der Benutzer authentifiziert ist
            var authState = await HttpContext.AuthenticateAsync();
            var isAuthenticated = authState.Succeeded;

            if (isAuthenticated && authState.Principal.Identity.Name == foodItem.User?.UserName)
            {
                // Der Benutzer ist authentifiziert und der Eigentümer des Artikels, Views nicht erhöhen
            }
            else
            {
                // Der Benutzer ist nicht der Eigentümer des Artikels, erhöhe die Views
                foodItem.Views++;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(new FoodItemResult { Successful = false, ErrorMessage = ex.Message });
                }
            }

            var category = (ShareIt.Shared.Category)foodItem.Category;

            var foodItemModel = new FoodItemModelWithId
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Price = foodItem.Price,
                Description = foodItem.Description,
                ImageUrl = foodItem.ImageUrl,
                UserName = foodItem.User?.UserName,
                MHD = foodItem.MHD,
                Category = category,
                Views = foodItem.Views,
                Street = foodItem.Street,
                PostalCode = foodItem.PostalCode,
                City = foodItem.City
            };

            return Ok(foodItemModel);
        }



        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetFoodItemsByCategory(int category, [FromQuery] string? excludeUser)
        {
            var foodItems = await _context.FoodItems
                .Include(f => f.User)
                .Where(f => f.Category == (DataObjects.Category)category)
                .ToListAsync();

            if (!string.IsNullOrEmpty(excludeUser))
            {
                foodItems = foodItems
                    .Where(f => f.User?.UserName != excludeUser)
                    .ToList();
            }

            var foodItemModels = foodItems.Select(foodItem => new FoodItemModelWithId
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Price = foodItem.Price,
                Description = foodItem.Description,
                ImageUrl = foodItem.ImageUrl,
                UserName = foodItem.User?.UserName,
                MHD = foodItem.MHD,
                Category = (ShareIt.Shared.Category)foodItem.Category,
                Views = foodItem.Views,
                Street = foodItem.Street,
                PostalCode = foodItem.PostalCode,
                City = foodItem.City
            }).ToList();

            return Ok(foodItemModels);
        }


        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetFoodItemsByUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("Benutzer nicht gefunden.");
            }

            var foodItems = await _context.FoodItems
                .Include(f => f.User)
                .Where(f => f.User.UserName == userName)
                .ToListAsync();


            var foodItemModels = foodItems.Select(foodItem => new FoodItemModelWithId
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Price = foodItem.Price,
                Description = foodItem.Description,
                ImageUrl = foodItem.ImageUrl,
                UserName = foodItem.User?.UserName,
                MHD = foodItem.MHD,
                Category = (ShareIt.Shared.Category)foodItem.Category,
                Views = foodItem.Views
            }).ToList();

            return Ok(foodItemModels);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodItem(int id, [FromBody] FoodItemModel foodItemModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            foodItem.Name = foodItemModel.Name;
            foodItem.Price = foodItemModel.Price;
            foodItem.Description = foodItemModel.Description;
            foodItem.ImageUrl = foodItemModel.ImageUrl;
            foodItem.Category = (DataObjects.Category)foodItemModel.Category;
            foodItem.MHD = foodItemModel.MHD;
            foodItem.Street = foodItemModel.Street;
            foodItem.PostalCode = foodItemModel.PostalCode;
            foodItem.City = foodItemModel.City;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(foodItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItem(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            try
            {
                _context.FoodItems.Remove(foodItem);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Löschen des Food-Objekts: " + ex.Message);
                return BadRequest(new FoodItemResult { Successful = false, ErrorMessage = ex.Message });
            }
        }

        private bool FoodItemExists(int id)
        {
            return _context.FoodItems.Any(e => e.Id == id);
        }
    }
}
