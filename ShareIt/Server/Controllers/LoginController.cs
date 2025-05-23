﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShareIt.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShareIt.Server.Controllers
{
    // API-Controller für die Benutzeranmeldung.
    // Verifiziert Benutzeranmeldeinformationen und gibt bei Erfolg ein JWT-Token zurück.
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;

        // Initialisiert den LoginController mit Konfigurations- und SignInManager.
        public LoginController(IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration; 
            _signInManager = signInManager;
        }

        // Führt die Benutzeranmeldung anhand der übermittelten E-Mail und Passwort durch.
        // Gibt ein JWT zurück, wenn die Anmeldung erfolgreich ist.
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {

            var result = await _signInManager.PasswordSignInAsync(login.Email!, login.Password!, false, false);

            if (!result.Succeeded) return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, login.Email!)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
