using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareIt.Server.Data;
using ShareIt.Server.DataObjects;
using ShareIt.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            var foodItem = new FoodItem
            {
                Name = foodItemModel.Name,
                Price = foodItemModel.Price,
                Description = foodItemModel.Description,
                UserId = user.Id,
                ImageUrl = foodItemModel.ImageUrl // Set the ImageUrl property
            };

            try
            {
                _context.FoodItems.Add(foodItem);
                await _context.SaveChangesAsync();
                return Ok(foodItem);
            }
            catch (Exception ex)
            {
                // Error occurred while saving the food item, handle the error accordingly
                return BadRequest(new FoodItemResult { Successful = false, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodItem(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return Ok(foodItem);
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

            // Update properties of the foodItem entity
            foodItem.Name = foodItemModel.Name;
            foodItem.Price = foodItemModel.Price;
            foodItem.Description = foodItemModel.Description;

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

            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodItemExists(int id)
        {
            return _context.FoodItems.Any(e => e.Id == id);
        }
    }
}