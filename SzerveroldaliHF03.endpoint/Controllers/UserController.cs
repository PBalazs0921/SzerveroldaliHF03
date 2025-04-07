using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SzerveroldaliHF03.Entities.Dto;

namespace SzerveroldaliHF03.Controllers;




[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    
    private UserManager<IdentityUser> userManager;
    private RoleManager<IdentityRole> roleManager;
    private IConfiguration configuration;
    
    public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        this.configuration = configuration;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserCUDDto dto)
    {
        var user = new IdentityUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            // Return an error message if user creation failed
            return BadRequest(result.Errors);
        }

        // If user is the first user, assign them the Admin role
        if (userManager.Users.Count() == 1)
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            var addToRoleResult = await userManager.AddToRoleAsync(user, "Admin");
            if (!addToRoleResult.Succeeded)
            {
                return BadRequest(addToRoleResult.Errors);
            }
        }

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var result = await userManager.CheckPasswordAsync(user, dto.Password);
        if (result)
        {

            //todo: generate token
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var role in await userManager.GetRolesAsync(user))
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }

            int expiryInMinutes = 24 * 60;
            var token = GenerateAccessToken(claim, expiryInMinutes);

            return Ok(new LoginResultDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = DateTime.Now.AddMinutes(expiryInMinutes)
            });
        }
        else
        {
            throw new Exception("Invalid password");
        }
    }
    
    
    [HttpGet]
    [Authorize]
    public IEnumerable<IdentityUser> Get()
    {
        return userManager.Users.ToList();
    }
    
    
    private JwtSecurityToken GenerateAccessToken(IEnumerable<Claim>? claims, int expiryInMinutes)
    {
        var signinKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["jwt:key"] ?? throw new Exception("jwt:key not found in appsettings.json")));

        return new JwtSecurityToken(
            issuer: "movieclub.com",
            audience: "movieclub.com",
            claims: claims?.ToArray(),
            expires: DateTime.Now.AddMinutes(expiryInMinutes),
            signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
        );
    }
}
