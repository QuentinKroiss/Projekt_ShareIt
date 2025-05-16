using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareIt.Shared;

namespace ShareIt.Server.Controllers
{
    // Controller zur Verwaltung von Benutzerkonten.
    // Ermöglicht Registrierung und das Löschen eines Benutzers.
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        // Initialisiert eine neue Instanz des <see cref="AccountsController"/>.
        public AccountsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Registriert einen neuen Benutzer mit E-Mail und Passwort.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Email);

            if (existingUser != null)
            {
                var errorMessage = $"Der Username '{model.Email}' ist leider schon vergeben.";
                return BadRequest(new RegisterResult { Successful = false, Errors = new List<string> { errorMessage } });
            }

            var newUser = new IdentityUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(newUser, model.Password!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return BadRequest(new RegisterResult { Successful = false, Errors = errors });
            }

            return Ok(new RegisterResult { Successful = true });
        }

        // Löscht einen Benutzer anhand der E-Mail, wenn das Passwort korrekt ist.
        [HttpDelete("DeleteByEmail")]
        public async Task<IActionResult> DeleteByEmail(string email, [FromBody] DeleteUserModel model)
        {
            // Überprüfung von Email und Passwort
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("E-Mail not found");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return BadRequest("Invalid password");
            }

            // Benutzer löschen
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return BadRequest(errors);
            }

            return Ok(new DeleteUserResult { Successful = true });
        }
    }
}
